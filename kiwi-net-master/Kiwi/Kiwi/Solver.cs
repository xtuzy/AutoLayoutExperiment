using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace Kiwi
{
    public class Solver
    {
        #region Nested Types

        private class Tag // TODO: make struct?
        {
            public Symbol Marker;
            // NOTE: original cpp version doesn't have initialization
            // of other field in constructor, but this version fails 
            // sometimes if this field left uninitialized.
            // TODO: find out reason whats going wrong
            public Symbol Other = new Symbol();
        }

        private class EditInfo // TODO: make struct?
        {
            public Tag Tag;
            public Constraint Constraint;
            public double Constant;
        }

        // NOTE in original C++ version std::map is used which is analog to SortedDictionary in C#
        // TODO: need to understand how use of Dictionaty vs SortedDictionary actually affects the performance
        private class Map<TKey, TValue> : Dictionary<TKey, TValue>
        {
        }

        #endregion
        //	typedef MapType<Variable, Symbol>::Type VarMap;
        //	typedef MapType<Symbol, Row*>::Type RowMap;
        //	typedef MapType<Constraint, Tag>::Type CnMap;
        //	typedef MapType<Variable, EditInfo>::Type EditMap;
        private readonly Map<Constraint, Tag> _cns;
        private readonly Map<Symbol, Row> _rows;
        private readonly Map<Variable, Symbol> _vars;
        private readonly Map<Variable, EditInfo> _edits;
        private readonly Stack<Symbol> _infeasibleRows;
        private Row _objective;
        private Row _artificial;
        private int _idTick;
        //	CnMap m_cns;
        //	RowMap m_rows;
        //	VarMap m_vars;
        //	EditMap m_edits;
        //	std::vector<Symbol> m_infeasible_rows;
        //	std::auto_ptr<Row> m_objective;
        //	std::auto_ptr<Row> m_artificial;
        //	Symbol::Id m_id_tick;

        public Solver()
        {
            _cns = new Map<Constraint, Tag>();
            _rows = new Map<Symbol, Row>();
            _vars = new Map<Variable, Symbol>();
            _edits = new Map<Variable, EditInfo>();
            _infeasibleRows = new Stack<Symbol>();
            _objective = new Row();
            _idTick = 1;
        }
        //	SolverImpl() : m_objective( new Row() ), m_id_tick( 1 ) {}
        //	~SolverImpl() { clearRows(); }

        #region Public Methods

        /// <summary>
        ///     Add a constraint to the solver.
        /// </summary>
        /// <param name="constraint"></param>
        /// <exception cref="DuplicateConstraint">
        ///     The given constraint has already been added to the solver.
        /// </exception>
        /// <exception cref="UnsatisfiableConstraint">
        ///     The given constraint is required and cannot be satisfied.
        /// </exception>
        public void AddConstraint(Constraint constraint)
        {
            //Stopwatch timer = new Stopwatch();
            //timer.Start();
            if (_cns.ContainsKey(constraint))
            {
                throw new DuplicateConstraint(constraint);
            }
            //timer.Stop();
            //Debug.WriteLine("SpendTime:" + $"{timer.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:1");
            // Creating a row causes symbols to reserved for the variables
            // in the constraint. If this method exits with an exception,
            // then its possible those variables will linger in the var map.
            // Since its likely that those variables will be used in other
            // constraints and since exceptional conditions are uncommon,
            // i'm not too worried about aggressive cleanup of the var map.
            //timer.Restart();
            Tag tag = new Tag();
            Row row = CreateRow(constraint, tag);
            Symbol subject = ChooseSubject(row, tag);
            //timer.Stop();
            //Debug.WriteLine("SpendTime:" + $"{timer.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:2");
            // If chooseSubject could find a valid entering symbol, one
            // last option is available if the entire row is composed of
            // dummy variables. If the constant of the row is zero, then
            // this represents redundant constraints and the new dummy
            // marker can enter the basis. If the constant is non-zero,
            // then it represents an unsatisfiable constraint.
            if (subject.Type == SymbolType.Invalid && AllDummies(row))
            {
                if (!Row.nearZero(row.Constant))
                {
                    throw new UnsatisfiableConstraint(constraint);
                }
                subject = tag.Marker;
            }

            // If an entering symbol still isn't found, then the row must
            // be added using an artificial variable. If that fails, then
            // the row represents an unsatisfiable constraint.
            //timer.Restart();
            if (subject.Type == SymbolType.Invalid)
            {
                if (!AddWithArtificialVariable(row))
                {
                    throw new UnsatisfiableConstraint(constraint);
                }
            }
            else
            {
                //Stopwatch timer3 = new Stopwatch();
                //timer3.Start();
                //timer3.Restart();
                row.SolveFor(subject);
                //timer3.Stop();
                //Debug.WriteLine("SpendTime:" + $"{timer3.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:3-1");
                //timer3.Restart();
                Substitute(subject, row);
                //timer3.Stop();
                //Debug.WriteLine("SpendTime:" + $"{timer3.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:3-2");
                _rows[subject] = row;
            }

            _cns[constraint] = tag;

            //timer.Stop();
            //Debug.WriteLine("SpendTime:" + $"{timer.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:3");
            // Optimizing after each constraint is added performs less
            // aggregate work due to a smaller average system size. It
            // also ensures the solver remains in a consistent state.
            //timer.Restart();
            Optimize(_objective);
            //timer.Stop();
            //Debug.WriteLine("SpendTime:" + $"{timer.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:4");
        }
        //	void addConstraint( const Constraint& constraint )
            //	{
        //		if( m_cns.find( constraint ) != m_cns.end() )
        //			throw DuplicateConstraint( constraint );

        //		// Creating a row causes symbols to reserved for the variables
        //		// in the constraint. If this method exits with an exception,
        //		// then its possible those variables will linger in the var map.
        //		// Since its likely that those variables will be used in other
        //		// constraints and since exceptional conditions are uncommon,
        //		// i'm not too worried about aggressive cleanup of the var map.
        //		Tag tag;
        //		std::auto_ptr<Row> rowptr( createRow( constraint, tag ) );
        //		Symbol subject( chooseSubject( *rowptr, tag ) );

        //		// If chooseSubject could find a valid entering symbol, one
        //		// last option is available if the entire row is composed of
        //		// dummy variables. If the constant of the row is zero, then
        //		// this represents redundant constraints and the new dummy
        //		// marker can enter the basis. If the constant is non-zero,
        //		// then it represents an unsatisfiable constraint.
        //		if( subject.type() == Symbol::Invalid && allDummies( *rowptr ) )
        //		{
        //			if( !nearZero( rowptr->constant() ) )
        //				throw UnsatisfiableConstraint( constraint );
        //			else
        //				subject = tag.marker;
        //		}

        //		// If an entering symbol still isn't found, then the row must
        //		// be added using an artificial variable. If that fails, then
        //		// the row represents an unsatisfiable constraint.
        //		if( subject.type() == Symbol::Invalid )
        //		{
        //			if( !addWithArtificialVariable( *rowptr ) )
        //				throw UnsatisfiableConstraint( constraint );
        //		}
        //		else
        //		{
        //			rowptr->solveFor( subject );
        //			substitute( subject, *rowptr );
        //			m_rows[ subject ] = rowptr.release();
        //		}

        //		m_cns[ constraint ] = tag;

        //		// Optimizing after each constraint is added performs less
        //		// aggregate work due to a smaller average system size. It
        //		// also ensures the solver remains in a consistent state.
        //		optimize( *m_objective );
        //	}


        /// <summary>
        ///     Remove a constraint from the solver.
        /// </summary>
        /// <param name="constraint"></param>
        /// <exception cref="UnknownConstraint">The given constraint has not been added to the solver.</exception>
        public void RemoveConstraint(Constraint constraint)
        {
            if (!_cns.TryGetValue(constraint, out Tag tag))
            {
                throw new UnknownConstraint(constraint);
            }

            _cns.Remove(constraint);

            // Remove the error effects from the objective function
            // *before* pivoting, or substitutions into the objective
            // will lead to incorrect solver results.
            RemoveConstraintEffects(constraint, tag);
            
            // If the marker is basic, simply drop the row. Otherwise,
            // pivot the marker into the basis and then drop the row.
            if (!_rows.Remove(tag.Marker))
            {
                var row = GetMarkerLeavingRow(tag.Marker);
                if (row == null)
                {
                    throw new InternalSolverError("failed to find leaving row");
                }

                // TODO: remove kvp as return value
                Symbol leaving = row.Value.Key;
                Row rowptr = row.Value.Value;

                _rows.Remove(leaving);
                rowptr.SolveFor(leaving, tag.Marker);
                Substitute(tag.Marker, rowptr);
            }

            // Optimizing after each constraint is removed ensures that the
            // solver remains consistent. It makes the solver api easier to
            // use at a small tradeoff for speed.
            Optimize(_objective);
        }
        //	void removeConstraint( const Constraint& constraint )
        //	{
        //		CnMap::iterator cn_it = m_cns.find( constraint );
        //		if( cn_it == m_cns.end() )
        //			throw UnknownConstraint( constraint );

        //		Tag tag( cn_it->second );
        //		m_cns.erase( cn_it );

        //		// Remove the error effects from the objective function
        //		// *before* pivoting, or substitutions into the objective
        //		// will lead to incorrect solver results.
        //		removeConstraintEffects( constraint, tag );

        //		// If the marker is basic, simply drop the row. Otherwise,
        //		// pivot the marker into the basis and then drop the row.
        //		RowMap::iterator row_it = m_rows.find( tag.marker );
        //		if( row_it != m_rows.end() )
        //		{
        //			std::auto_ptr<Row> rowptr( row_it->second );
        //			m_rows.erase( row_it );
        //		}
        //		else
        //		{
        //			row_it = getMarkerLeavingRow( tag.marker );
        //			if( row_it == m_rows.end() )
        //				throw InternalSolverError( "failed to find leaving row" );
        //			Symbol leaving( row_it->first );
        //			std::auto_ptr<Row> rowptr( row_it->second );
        //			m_rows.erase( row_it );
        //			rowptr->solveFor( leaving, tag.marker );
        //			substitute( tag.marker, *rowptr );
        //		}

        //		// Optimizing after each constraint is removed ensures that the
        //		// solver remains consistent. It makes the solver api easier to
        //		// use at a small tradeoff for speed.
        //		optimize( *m_objective );
        //	}


        /// <summary>
        ///     Test whether a constraint has been added to the solver.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public bool HasConstraint(Constraint constraint)
        {
            return _cns.ContainsKey(constraint);
        }


        /// <summary>
        ///     Add an edit variable to the solver.
        ///     This method should be called before the `suggestValue` method is
        ///     used to supply a suggested value for the given edit variable.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="strength"></param>
        /// <exception cref="DuplicateEditVariable">The given edit variable has already been added to the solver.</exception>
        /// <exception cref="BadRequiredStrength">The given strength is >= required.</exception>
        public void AddEditVariable(Variable variable, double strength)
        {
            if (_edits.ContainsKey(variable))
            {
                throw new DuplicateEditVariable(variable);
            }

            strength = Strength.Clip(strength);
            if (strength == Strength.Required)
            {
                throw new BadRequiredStrength();
            }

            Constraint cn = new Constraint(new Expression(new Term(variable)), RelationalOperator.OP_EQ, strength);
            AddConstraint(cn);
            _edits[variable] = new EditInfo
            {
                Tag = _cns[cn],
                Constraint = cn,
                Constant = 0.0
            };
        }
        //	void addEditVariable( const Variable& variable, double strength )
        //	{
        //		if( m_edits.find( variable ) != m_edits.end() )
        //			throw DuplicateEditVariable( variable );
        //		strength = strength::clip( strength );
        //		if( strength == strength::required )
        //			throw BadRequiredStrength();
        //		Constraint cn( Expression( variable ), OP_EQ, strength );
        //		addConstraint( cn );
        //		EditInfo info;
        //		info.tag = m_cns[ cn ];
        //		info.constraint = cn;
        //		info.constant = 0.0;
        //		m_edits[ variable ] = info;
        //	}


        /// <summary>
        ///     Remove an edit variable from the solver.
        /// </summary>
        /// <param name="variable">The variable to remove.</param>
        /// <exception cref="UnknownEditVariable">The given edit variable has not been added to the solver.</exception>
        public void RemoveEditVariable(Variable variable)
        {
            if (!_edits.TryGetValue(variable, out var edit))
                throw new UnknownEditVariable(variable);

            RemoveConstraint(edit.Constraint);
            _edits.Remove(variable);
        }

        /// <summary>
        ///     Test whether an edit variable has been added to the solver.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public bool HasEditVariable(Variable variable)
        {
            return _edits.ContainsKey(variable);
        }


        /// <summary>
        ///     Suggest a value for the given edit variable.
        ///     This method should be used after an edit variable has been added to
        ///     the solver in order to suggest the value for that variable. After
        ///     all suggestions have been made, the `solve` method can be used to
        ///     update the values of all variables.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        /// <exception cref="UnknownEditVariable">The given edit variable has not been added to the solver.</exception>
        public void SuggestValue(Variable variable, double value)
        {
            if (!_edits.TryGetValue(variable, out EditInfo info))
            {
                throw new UnknownEditVariable(variable);
            }

            using (new DualOptimizeGuard(this))
            {
                double delta = value - info.Constant;
                info.Constant = value;

                // Check first if the positive error variable is basic.
                if (_rows.TryGetValue(info.Tag.Marker, out Row row))
                {
                    if (row.Add(-delta) < 0.0)
                    {
                        _infeasibleRows.Push(info.Tag.Marker);
                    }
                    return;
                }

                // Check next if the negative error variable is basic.
                if (_rows.TryGetValue(info.Tag.Other, out Row otherRow))
                {
                    if (otherRow.Add(delta) < 0.0)
                    {
                        _infeasibleRows.Push(info.Tag.Other);
                    }
                    return;
                }

                // Otherwise update each row where the error variables exist.
                foreach (var row_it in _rows)
                {
                    double coeff = row_it.Value.CoefficientFor(info.Tag.Marker);
                    if (coeff != 0.0 &&
                        row_it.Value.Add(delta * coeff) < 0.0 &&
                        row_it.Key.Type != SymbolType.External)
                    {
                        _infeasibleRows.Push(row_it.Key);
                    }
                }
            }
        }
        //	void suggestValue( const Variable& variable, double value )
        //	{
        //		EditMap::iterator it = m_edits.find( variable );
        //		if( it == m_edits.end() )
        //			throw UnknownEditVariable( variable );

        //		DualOptimizeGuard guard( *this );
        //		EditInfo& info = it->second;
        //		double delta = value - info.constant;
        //		info.constant = value;

        //		// Check first if the positive error variable is basic.
        //		RowMap::iterator row_it = m_rows.find( info.tag.marker );
        //		if( row_it != m_rows.end() )
        //		{
        //			if( row_it->second->add( -delta ) < 0.0 )
        //				m_infeasible_rows.push_back( row_it->first );
        //			return;
        //		}

        //		// Check next if the negative error variable is basic.
        //		row_it = m_rows.find( info.tag.other );
        //		if( row_it != m_rows.end() )
        //		{
        //			if( row_it->second->add( delta ) < 0.0 )
        //				m_infeasible_rows.push_back( row_it->first );
        //			return;
        //		}

        //		// Otherwise update each row where the error variables exist.
        //		RowMap::iterator end = m_rows.end();
        //		for( row_it = m_rows.begin(); row_it != end; ++row_it )
        //		{
        //			double coeff = row_it->second->coefficientFor( info.tag.marker );
        //			if( coeff != 0.0 &&
        //				row_it->second->add( delta * coeff ) < 0.0 &&
        //				row_it->first.type() != Symbol::External )
        //				m_infeasible_rows.push_back( row_it->first );
        //		}
        //	}

        /// <summary>
        ///     Update the values of the external solver variables.
        /// </summary>
        public void UpdateVariables()
        {
            foreach (Variable var in _vars.Keys)
            {
                var.Value = _rows.TryGetValue(_vars[var], out Row row) ? row.Constant : 0.0;
            }
        }
        //	void updateVariables()
        //	{
        //		typedef RowMap::iterator row_iter_t;
        //		typedef VarMap::iterator var_iter_t;
        //		row_iter_t row_end = m_rows.end();
        //		var_iter_t var_end = m_vars.end();
        //		for( var_iter_t var_it = m_vars.begin(); var_it != var_end; ++var_it )
        //		{
        //			Variable& var( const_cast<Variable&>( var_it->first ) );
        //			row_iter_t row_it = m_rows.find( var_it->second );
        //			if( row_it == row_end )
        //				var.setValue( 0.0 );
        //			else
        //				var.setValue( row_it->second->constant() );
        //		}
        //	}


        /// <summary>
        ///     Reset the solver to the empty starting condition.
        /// </summary>
        /// <remarks>
        ///     This method resets the internal solver state to the empty starting
        ///     condition, as if no constraints or edit variables have been added.
        ///     This can be faster than deleting the solver and creating a new one
        ///     when the entire system must change, since it can avoid unecessary
        ///     heap(de)allocations.
        /// </remarks>
        public void Reset()
        {
            _cns.Clear();
            _rows.Clear();
            _vars.Clear();
            _edits.Clear();
            _infeasibleRows.Clear();
            _objective = new Row();
            _artificial = null;
            _idTick = 1;
        }


        /// <summary>
        ///     Dump a representation of the solver internals to stdout.
        /// </summary>
        public void Dump()
        {
        }

        #endregion

        #region Private Methods

        private void ClearRows() // TODO inline
        {
            _rows.Clear();
        }
        //	void clearRows()
        //	{
        //		std::for_each( m_rows.begin(), m_rows.end(), RowDeleter() );
        //		m_rows.clear();
        //	}


        /* Get the symbol for the given variable.
        If a symbol does not exist for the variable, one will be created.
        */
        private Symbol GetVarSymbol(Variable variable)
        {
            if (_vars.TryGetValue(variable, out var symbol)) return symbol;
            symbol = new Symbol(SymbolType.External, _idTick++);
            return _vars[variable] = symbol;
        }
        //	{
        //		VarMap::iterator it = m_vars.find( variable );
        //		if( it != m_vars.end() )
        //			return it->second;
        //		Symbol symbol( Symbol::External, m_id_tick++ );
        //		m_vars[ variable ] = symbol;
        //		return symbol;
        //	}

        /// <summary>
        /// Create a new Row object for the given constraint.<br/>
        ///
        /// The terms in the constraint will be converted to cells in the row.
        /// Any term in the constraint with a coefficient of zero is ignored.
        /// This method uses the `getVarSymbol` method to get the symbol for
        /// the variables added to the row. If the symbol for a given cell
        /// variable is basic, the cell variable will be substituted with the
        /// basic row.<br/>
        ///
        /// The necessary slack and error variables will be added to the row.
        /// If the constant for the row is negative, the sign for the row
        /// will be inverted so the constant becomes positive.<br/>
        ///
        /// The tag will be updated with the marker and error symbols to use
        /// for tracking the movement of the constraint in the tableau.<br/>
        /// 
        /// 翻译<br/>
        /// 为被给的约束创建一个新的<see cref="Row"/>.<br/>
        /// <br/>
        /// 约束中的<see cref="Term"/>会被转换成<see cref="Row"/>中的<see cref="Row.Cells"/>.<br/>
        /// 约束中,任何系数为0的<see cref="Term"/>都会被忽略.<br/>
        /// 这个方法使用<see cref="GetVarSymbol(Variable)"/>方法为被添加到<see cref="Row"/>的<see cref="Variable"/>获取<see cref="Symbol"/>.<br/>
        /// 如果这个<see cref="Symbol"/>for一个被给的cell variable是基础的,那么这个cell variable会被用基础row替代.<br/>
        /// <br/>
        /// 这必要的<see cref="SymbolType.Slack"/> <see cref="Variable"/>和<see cref="SymbolType.Error"/> <see cref="Variable"/>会被添加到<see cref="Row"/>.<br/>
        /// 如果这个<see cref="Constraint"/>对于这个<see cref="Row"/>是负的,这个Row的Sign会被反转,导致Constant变成正的.<br/>
        /// <br/>
        /// 
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private Row CreateRow(Constraint constraint, Tag tag )
        {
            var expr = constraint.Expression;
            var row = new Row(expr.Constant);

            // Substitute the current basic variables into the row.
            foreach (var term in expr.Terms)
            {
                if (!Row.nearZero(term.Coefficient))
                {
                    Symbol symbol = GetVarSymbol(term.Variable);
                    if (_rows.TryGetValue(symbol, out Row row_it))
                    {
                        row.Insert(row_it, term.Coefficient);
                    }
                    else
                    {
                        row.Insert(symbol, term.Coefficient);
                    }
                }
            }

            // Add the necessary slack, error, and dummy variables.
            switch (constraint.Op)
            {
                case RelationalOperator.OP_LE:
                case RelationalOperator.OP_GE:
                {
                        var coeff = constraint.Op == RelationalOperator.OP_LE ? 1.0 : -1.0;
                        var slack = new Symbol(SymbolType.Slack, _idTick++);
                        tag.Marker = slack;
                        row.Insert(slack, coeff);
                        if (constraint.Strength < Strength.Required)
                        {
                            var error = new Symbol(SymbolType.Error, _idTick++ );
                            tag.Other = error;
                            row.Insert(error, -coeff);
                            _objective.Insert(error, constraint.Strength);
                        }
                        break;
                }
                case RelationalOperator.OP_EQ:
                {
                    if (constraint.Strength < Strength.Required)
                    {
                        var errplus = new Symbol(SymbolType.Error, _idTick++);
                        var errminus = new Symbol(SymbolType.Error, _idTick++);
                        tag.Marker = errplus;
                        tag.Other = errminus;
                        row.Insert(errplus, -1.0); // v = eplus - eminus
                        row.Insert(errminus, 1.0); // v - eplus + eminus = 0
                        _objective.Insert(errplus, constraint.Strength);
                        _objective.Insert(errminus, constraint.Strength);
                    }
                    else
                    {
                        var dummy = new Symbol(SymbolType.Dummy, _idTick++);
                        tag.Marker = dummy;
                        row.Insert(dummy);
                    }
                    break;
                }
            }

            // Ensure the row as a positive constant.
            if (row.Constant < 0.0)
                row.ReverseSign();

            return row;
        }
        //	Row* createRow( const Constraint& constraint, Tag& tag )
        //	{
        //		typedef std::vector<Term>::const_iterator iter_t;
        //		const Expression& expr( constraint.expression() );
        //		Row* row = new Row( expr.constant() );

        //		// Substitute the current basic variables into the row.
        //		iter_t end = expr.terms().end();
        //		for( iter_t it = expr.terms().begin(); it != end; ++it )
        //		{
        //			if( !nearZero( it->coefficient() ) )
        //			{
        //				Symbol symbol( getVarSymbol( it->variable() ) );
        //				RowMap::const_iterator row_it = m_rows.find( symbol );
        //				if( row_it != m_rows.end() )
        //					row->insert( *row_it->second, it->coefficient() );
        //				else
        //					row->insert( symbol, it->coefficient() );
        //			}
        //		}

        //		// Add the necessary slack, error, and dummy variables.
        //		switch( constraint.op() )
        //		{
        //			case OP_LE:
        //			case OP_GE:
        //			{
        //				double coeff = constraint.op() == OP_LE ? 1.0 : -1.0;
        //				Symbol slack( Symbol::Slack, m_id_tick++ );
        //				tag.marker = slack;
        //				row->insert( slack, coeff );
        //				if( constraint.strength() < strength::required )
        //				{
        //					Symbol error( Symbol::Error, m_id_tick++ );
        //					tag.other = error;
        //					row->insert( error, -coeff );
        //					m_objective->insert( error, constraint.strength() );
        //				}
        //				break;
        //			}
        //			case OP_EQ:
        //			{
        //				if( constraint.strength() < strength::required )
        //				{
        //					Symbol errplus( Symbol::Error, m_id_tick++ );
        //					Symbol errminus( Symbol::Error, m_id_tick++ );
        //					tag.marker = errplus;
        //					tag.other = errminus;
        //					row->insert( errplus, -1.0 ); // v = eplus - eminus
        //					row->insert( errminus, 1.0 ); // v - eplus + eminus = 0
        //					m_objective->insert( errplus, constraint.strength() );
        //					m_objective->insert( errminus, constraint.strength() );
        //				}
        //				else
        //				{
        //					Symbol dummy( Symbol::Dummy, m_id_tick++ );
        //					tag.marker = dummy;
        //					row->insert( dummy );
        //				}
        //				break;
        //			}
        //		}

        //		// Ensure the row as a positive constant.
        //		if( row->constant() < 0.0 )
        //			row->reverseSign();

        //		return row;
        //	}


        //	Choose the subject for solving for the row.
        //
        //	This method will choose the best subject for using as the solve
        //	target for the row. An invalid symbol will be returned if there
        //	is no valid target.
        //
        //	The symbols are chosen according to the following precedence:
        //
        //	1) The first symbol representing an external variable.
        //	2) A negative slack or error tag variable.
        //
        //	If a subject cannot be found, an invalid symbol will be returned.
        private Symbol ChooseSubject(Row row, Tag tag)
        {
            foreach (var symbol in row.Cells.Keys)
            {
                if (symbol.Type == SymbolType.External)
                {
                    return symbol;
                }
            }

            if (tag.Marker.Type == SymbolType.Slack ||
                tag.Marker.Type == SymbolType.Error)
            {
                if (row.CoefficientFor(tag.Marker) < 0.0) return tag.Marker;
            }


            // TODO tag.Other != null
            if (tag.Other.Type == SymbolType.Slack ||
                tag.Other.Type == SymbolType.Error)
            {
                if (row.CoefficientFor(tag.Other) < 0.0) return tag.Other;
            }

            return new Symbol();
        }
        //	{
        //		typedef Row::CellMap::const_iterator iter_t;
        //		iter_t end = row.cells().end();
        //		for( iter_t it = row.cells().begin(); it != end; ++it )
        //		{
        //			if( it->first.type() == Symbol::External )
        //				return it->first;
        //		}
        //		if( tag.marker.type() == Symbol::Slack || tag.marker.type() == Symbol::Error )
        //		{
        //			if( row.coefficientFor( tag.marker ) < 0.0 )
        //				return tag.marker;
        //		}
        //		if( tag.other.type() == Symbol::Slack || tag.other.type() == Symbol::Error )
        //		{
        //			if( row.coefficientFor( tag.other ) < 0.0 )
        //				return tag.other;
        //		}
        //		return Symbol();
        //	}


        // 	Add the row to the tableau using an artificial variable.
        //
        //	This will return false if the constraint cannot be satisfied.
        private bool AddWithArtificialVariable(Row row)
        {
            // Create and add the artificial variable to the tableau
            var art = new Symbol(SymbolType.Slack, _idTick++);
            _rows[art] = new Row(row);
            _artificial = new Row(row);

            // Optimize the artificial objective. This is successful
            // only if the artificial objective is optimized to zero.
            Optimize(_artificial);
            bool success = Row.nearZero(_artificial.Constant);
            _artificial = null;
            
            // TODO: saving _artificial as temp state is not nice

            // If the artificial variable is basic, pivot the row so that
            // it becomes basic. If the row is constant, exit early.
            if (_rows.TryGetValue(art, out Row rowptr))
            {
                _rows.Remove(art);
                if (rowptr.Cells.Count == 0)
                {
                    return success;
                }

                Symbol entering = AnyPivotableSymbol(rowptr);
                if (entering.Type == SymbolType.Invalid)
                {
                    return false;  // unsatisfiable (will this ever happen?)
                }
                rowptr.SolveFor(art, entering);
                Substitute(entering, rowptr);
                _rows[entering] = rowptr;
            }

            // Remove the artificial variable from the tableau.
            foreach (var it in _rows)
            {
                it.Value.Remove(art);
            }
            _objective.Remove(art);
            return success;
        }
        // 	{
        //		// Create and add the artificial variable to the tableau
        //		Symbol art( Symbol::Slack, m_id_tick++ );
        //		m_rows[ art ] = new Row( row );
        //		m_artificial.reset( new Row( row ) );

        //		// Optimize the artificial objective. This is successful
        //		// only if the artificial objective is optimized to zero.
        //		optimize( *m_artificial );
        //		bool success = nearZero( m_artificial->constant() );
        //		m_artificial.reset();

        //		// If the artificial variable is basic, pivot the row so that
        //		// it becomes basic. If the row is constant, exit early.
        //		RowMap::iterator it = m_rows.find( art );
        //		if( it != m_rows.end() )
        //		{
        //			std::auto_ptr<Row> rowptr( it->second );
        //			m_rows.erase( it );
        //			if( rowptr->cells().empty() )
        //				return success;
        //			Symbol entering( anyPivotableSymbol( *rowptr ) );
        //			if( entering.type() == Symbol::Invalid )
        //				return false;  // unsatisfiable (will this ever happen?)
        //			rowptr->solveFor( art, entering );
        //			substitute( entering, *rowptr );
        //			m_rows[ entering ] = rowptr.release();
        //		}

        //		// Remove the artificial variable from the tableau.
        //		RowMap::iterator end = m_rows.end();
        //		for( it = m_rows.begin(); it != end; ++it )
        //			it->second->remove( art );
        //		m_objective->remove( art );
        //		return success;
        // 	}


        /// <summary>
        /// Substitute the parametric symbol with the given row.
        /// This method will substitute all instances of the parametric symbol
        /// in the tableau and the objective function with the given row.
        /// <br/>
        /// 翻译<br/>
        /// 使用被给的Row替代Symbol.<br/>
        /// 这个方法会使用被给的Row替换在tableau和objective function中的全部Symbol实例.
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="row"></param>
        private void Substitute(Symbol symbol, Row row)
        {
            //Stopwatch timer32 = new Stopwatch();
            //timer32.Start();
            foreach (var it in _rows)
            {
                //Stopwatch timer321 = new Stopwatch();
                //timer321.Start();
                it.Value.Substitude(symbol, row);
                //timer321.Stop();
                //Debug.WriteLine("SpendTime:" + $"{timer321.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:3-2-1-1");
                //timer321.Restart();
                if (it.Key.Type != SymbolType.External && it.Value.Constant < 0.0)
                {
                    _infeasibleRows.Push(it.Key);
                }
                //timer321.Stop();
                //Debug.WriteLine("SpendTime:" + $"{timer321.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:3-2-1-2");
            }
            //timer32.Stop();
            //Debug.WriteLine("SpendTime:" + $"{timer32.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:3-2-1");
            //timer32.Restart();
            _objective.Substitude(symbol, row);
            //timer32.Stop();
            //Debug.WriteLine("SpendTime:" + $"{timer32.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:3-2-2");
            //timer32.Restart();
            _artificial?.Substitude(symbol, row);
            //timer32.Stop();
            //Debug.WriteLine("SpendTime:" + $"{timer32.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:3-2-3");
            //timer32.Restart();
        }
        //	{
        //		typedef RowMap::iterator iter_t;
        //		iter_t end = m_rows.end();
        //		for( iter_t it = m_rows.begin(); it != end; ++it )
        //		{
        //			it->second->substitute( symbol, row );
        //			if( it->first.type() != Symbol::External &&
        //				it->second->constant() < 0.0 )
        //				m_infeasible_rows.push_back( it->first );
        //		}
        //		m_objective->substitute( symbol, row );
        //		if( m_artificial.get() )
        //			m_artificial->substitute( symbol, row );
        //	}


        // Optimize the system for the given objective function.
        //
        // This method performs iterations of Phase 2 of the simplex method
        // until the objective function reaches a minimum.
        //
        // Throws
        // ------
        // InternalSolverError
        //      The value of the objective function is unbounded.
        private void Optimize(Row objective)
        {
            while (true)
            {
                var entering = GetEnteringSymbol(objective);
                if (entering.Type == SymbolType.Invalid)
                {
                    return;
                }

                var it = GetLeavingRow(entering);
                if (it == null)
                {
                    throw new InternalSolverError("The objective is unbounded.");
                }

                // pivot the entering symbol into the basis
                Symbol leaving = it.Value.Key;
                Row row = it.Value.Value;

                _rows.Remove(leaving);
                row.SolveFor(leaving, entering);
                Substitute(entering, row);
                _rows[entering] = row;
            }
        }
        //	{
        //		while( true )
        //		{
        //			Symbol entering( getEnteringSymbol( objective ) );
        //			if( entering.type() == Symbol::Invalid )
        //				return;
        //			RowMap::iterator it = getLeavingRow( entering );
        //			if( it == m_rows.end() )
        //				throw InternalSolverError( "The objective is unbounded." );
        //			// pivot the entering symbol into the basis
        //			Symbol leaving( it->first );
        //			Row* row = it->second;
        //			m_rows.erase( it );
        //			row->solveFor( leaving, entering );
        //			substitute( entering, *row );
        //			m_rows[ entering ] = row;
        //		}
        //	}



        //	Optimize the system using the dual of the simplex method.
        //
        //	The current state of the system should be such that the objective
        //	function is optimal, but not feasible. This method will perform
        //	an iteration of the dual simplex method to make the solution both
        //	optimal and feasible.
        //
        //	Throws
        //	------
        //	InternalSolverError
        //		The system cannot be dual optimized.
        private void DualOptimize()
        {
            while (_infeasibleRows.Count > 0)
            {
                Symbol leaving = _infeasibleRows.Pop();

                if (_rows.TryGetValue(leaving, out Row row) && row.Constant < 0.0)
                {
                    Symbol entering = GetDualEnteringSymbol(row);
                    if (entering.Type == SymbolType.Invalid)
                    {
                        throw new InternalSolverError("Dual optimize failed.");
                    }

                    // pivot the entering symbol into the basis
                    _rows.Remove(leaving);
                    row.SolveFor(leaving, entering);
                    Substitute(entering, row);
                    _rows[entering] = row;
                }
            }
        }
        //	*/
        //	void dualOptimize()
        //	{
        //		while( !m_infeasible_rows.empty() )
        //		{

        //			Symbol leaving( m_infeasible_rows.back() );
        //			m_infeasible_rows.pop_back();
        //			RowMap::iterator it = m_rows.find( leaving );
        //			if( it != m_rows.end() && it->second->constant() < 0.0 )
        //			{
        //				Symbol entering( getDualEnteringSymbol( *it->second ) );
        //				if( entering.type() == Symbol::Invalid )
        //					throw InternalSolverError( "Dual optimize failed." );
        //				// pivot the entering symbol into the basis
        //				Row* row = it->second;
        //				m_rows.erase( it );
        //				row->solveFor( leaving, entering );
        //				substitute( entering, *row );
        //				m_rows[ entering ] = row;
        //			}
        //		}
        //	}




        //	Compute the entering variable for a pivot operation.
        //
        //	This method will return first symbol in the objective function which
        //	is non-dummy and has a coefficient less than zero. If no symbol meets
        //	the criteria, it means the objective function is at a minimum, and an
        //	invalid symbol is returned.
        private Symbol GetEnteringSymbol(Row objective)
        {
            // TODO use tuples
            foreach (var entry in objective.Cells)
            {
                Symbol symbol = entry.Key;
                double value = entry.Value; // TODO name better than value

                if (symbol.Type != SymbolType.Dummy && value < 0.0)
                {
                    return symbol;
                }
            }

            return Symbol.Invalid;
        }
        //	*/
        //	Symbol getEnteringSymbol( const Row& objective )
        //	{
        //		typedef Row::CellMap::const_iterator iter_t;
        //		iter_t end = objective.cells().end();
        //		for( iter_t it = objective.cells().begin(); it != end; ++it )
        //		{
        //			if( it->first.type() != Symbol::Dummy && it->second < 0.0 )
        //				return it->first;
        //		}
        //		return Symbol();
        //	}



        //	Compute the entering symbol for the dual optimize operation.
        //
        //	This method will return the symbol in the row which has a positive
        //	coefficient and yields the minimum ratio for its respective symbol
        //	in the objective function. The provided row *must* be infeasible.
        //	If no symbol is found which meats the criteria, an invalid symbol
        //	is returned.
        private Symbol GetDualEnteringSymbol(Row row)
        {
            Symbol entering = new Symbol(); // TODO make value type?
            var ratio = double.MaxValue;

            foreach (var entry in row.Cells)
            {
                Symbol symbol = entry.Key;
                double value = entry.Value; // TODO name better than value

                if (value > 0.0 && symbol.Type != SymbolType.Dummy)
                {
                    double coeff = _objective.CoefficientFor(symbol);
                    double r = coeff / value;
                    if (r < ratio)
                    {
                        ratio = r;
                        entering = symbol;
                    }
                }
            }

            return entering;
        }
        //	Symbol getDualEnteringSymbol( const Row& row )
        //	{
        //		typedef Row::CellMap::const_iterator iter_t;
        //		Symbol entering;
        //		double ratio = std::numeric_limits<double>::max();
        //		iter_t end = row.cells().end();
        //		for( iter_t it = row.cells().begin(); it != end; ++it )
        //		{
        //			if( it->second > 0.0 && it->first.type() != Symbol::Dummy )
        //			{
        //				double coeff = m_objective->coefficientFor( it->first );
        //				double r = coeff / it->second;
        //				if( r < ratio )
        //				{
        //					ratio = r;
        //					entering = it->first;
        //				}
        //			}
        //		}
        //		return entering;
        //	}



        //	Get the first Slack or Error symbol in the row.
        //
        //	If no such symbol is present, and Invalid symbol will be returned.
        private Symbol AnyPivotableSymbol(Row row)
        {
            foreach (var symbol in row.Cells.Keys)
            {
                if (symbol.Type == SymbolType.Slack ||
                    symbol.Type == SymbolType.Error)
                {
                    return symbol;
                }
            }
            return new Symbol();
        }
        //	Symbol anyPivotableSymbol( const Row& row )
        //	{
        //		typedef Row::CellMap::const_iterator iter_t;
        //		iter_t end = row.cells().end();
        //		for( iter_t it = row.cells().begin(); it != end; ++it )
        //		{
        //			const Symbol& sym( it->first );
        //			if( sym.type() == Symbol::Slack || sym.type() == Symbol::Error )
        //				return sym;
        //		}
        //		return Symbol();
        //	}


        
        //	Compute the row which holds the exit symbol for a pivot.
        //
        //	This method will return an iterator to the row in the row map
        //	which holds the exit symbol. If no appropriate exit symbol is
        //	found, the end() iterator will be returned. This indicates that
        //	the objective function is unbounded.
        KeyValuePair<Symbol, Row>? GetLeavingRow(Symbol entering)
        {
            var ratio = double.MaxValue;
            KeyValuePair<Symbol, Row>? found = null;

            foreach (var it in _rows)
            {
                if (it.Key.Type != SymbolType.External)
                {
                    double temp = it.Value.CoefficientFor(entering);
                    if (temp < 0.0)
                    {
                        double temp_ratio = -it.Value.Constant / temp;
                        if (temp_ratio < ratio)
                        {
                            ratio = temp_ratio;
                            found = it;
                        }
                    }
                }
            }
            return found;
        }
        //	RowMap::iterator getLeavingRow( const Symbol& entering )
        //	{
        //		typedef RowMap::iterator iter_t;
        //		double ratio = std::numeric_limits<double>::max();
        //		iter_t end = m_rows.end();
        //		iter_t found = m_rows.end();
        //		for( iter_t it = m_rows.begin(); it != end; ++it )
        //		{
        //			if( it->first.type() != Symbol::External )
        //			{
        //				double temp = it->second->coefficientFor( entering );
        //				if( temp < 0.0 )
        //				{
        //					double temp_ratio = -it->second->constant() / temp;
        //					if( temp_ratio < ratio )
        //					{
        //						ratio = temp_ratio;
        //						found = it;
        //					}
        //				}
        //			}
        //		}
        //		return found;
        //	}



        //  Compute the leaving row for a marker variable.
        //
        //	This method will return an iterator to the row in the row map
        //	which holds the given marker variable. The row will be chosen
        //	according to the following precedence:
        //
        //	1) The row with a restricted basic varible and a negative coefficient
        //	   for the marker with the smallest ratio of -constant / coefficient.
        //
        //	2) The row with a restricted basic variable and the smallest ratio
        //	   of constant / coefficient.
        //
        //	3) The last unrestricted row which contains the marker.
        //
        //	If the marker does not exist in any row, the row map end() iterator
        //	will be returned. This indicates an internal solver error since
        //	the marker *should* exist somewhere in the tableau.
        private KeyValuePair<Symbol, Row>? GetMarkerLeavingRow(Symbol marker)
        {
            var r1 = double.MaxValue;
            var r2 = double.MaxValue;
            KeyValuePair<Symbol, Row>? first = null;
            KeyValuePair<Symbol, Row>? second = null;
            KeyValuePair<Symbol, Row>? third = null;

            foreach (var it in _rows)
            {
                double c = it.Value.CoefficientFor(marker);

                if (c == 0.0) continue;

                if (it.Key.Type == SymbolType.External)
                {
                    third = it;
                }
                else if (c < 0.0)
                {
                    double r = -it.Value.Constant / c;
                    if (r < r1)
                    {
                        r1 = r;
                        first = it;
                    }
                }
                else
                {
                    double r = it.Value.Constant / c;
                    if (r < r2)
                    {
                        r2 = r;
                        second = it;
                    }
                }
            }

            if (first.HasValue) return first.Value;
            if (second.HasValue) return second.Value;
            return third;
        }
        //	*/
        //	RowMap::iterator getMarkerLeavingRow( const Symbol& marker )
        //	{
        //		const double dmax = std::numeric_limits<double>::max();
        //		typedef RowMap::iterator iter_t;
        //		double r1 = dmax;
        //		double r2 = dmax;
        //		iter_t end = m_rows.end();
        //		iter_t first = end;
        //		iter_t second = end;
        //		iter_t third = end;
        //		for( iter_t it = m_rows.begin(); it != end; ++it )
        //		{
        //			double c = it->second->coefficientFor( marker );
        //			if( c == 0.0 )
        //				continue;
        //			if( it->first.type() == Symbol::External )
        //			{
        //				third = it;
        //			}
        //			else if( c < 0.0 )
        //			{
        //				double r = -it->second->constant() / c;
        //				if( r < r1 )
        //				{
        //					r1 = r;
        //					first = it;
        //				}
        //			}
        //			else
        //			{
        //				double r = it->second->constant() / c;
        //				if( r < r2 )
        //				{
        //					r2 = r;
        //					second = it;
        //				}
        //			}
        //		}
        //		if( first != end )
        //			return first;
        //		if( second != end )
        //			return second;
        //		return third;
        //	}

        // Remove the effects of a constraint on the objective function.
        private void RemoveConstraintEffects(Constraint cn, Tag tag)
        {
            if (tag.Marker.Type == SymbolType.Error) RemoveMarkerEffects(tag.Marker, cn.Strength);
            if (tag.Other.Type == SymbolType.Error) RemoveMarkerEffects(tag.Other, cn.Strength);
        }
        //void removeConstraintEffects( const Constraint& cn, const Tag& tag )
        //{
        //	if( tag.marker.type() == Symbol::Error )
        //		removeMarkerEffects( tag.marker, cn.strength() );
        //	if( tag.other.type() == Symbol::Error )
        //		removeMarkerEffects( tag.other, cn.strength() );
        //}


        // Remove the effects of an error marker on the objective function.
        private void RemoveMarkerEffects(Symbol marker, double strength)
        {
            if (_rows.TryGetValue(marker, out var row))
            {
                _objective.Insert(row, -strength);
            }
            else
            {
                _objective.Insert(marker, -strength);
            }
        }
        //void removeMarkerEffects( const Symbol& marker, double strength )
        //{
        //	RowMap::iterator row_it = m_rows.find( marker );
        //	if( row_it != m_rows.end() )
        //		m_objective->insert( *row_it->second, -strength );
        //	else
        //		m_objective->insert( marker, -strength );
        //}


        // Test whether a row is composed of all dummy variables.
        private bool AllDummies(Row row)
        {
            foreach (var symbol in row.Cells.Keys)
            {
                if (symbol.Type != SymbolType.Dummy) return false;
            }
            return true;
        }
        //bool allDummies( const Row& row )
        //{
        //	typedef Row::CellMap::const_iterator iter_t;
        //	iter_t end = row.cells().end();
        //	for( iter_t it = row.cells().begin(); it != end; ++it )
        //	{
        //		if( it->first.type() != Symbol::Dummy )
        //			return false;
        //	}
        //	return true;
        //}


        #endregion

        internal struct DualOptimizeGuard : IDisposable
        {
            private readonly Solver _solver;
            public DualOptimizeGuard(Solver solver) => _solver = solver;
            public void Dispose() => _solver.DualOptimize();
        }
    }


    //	struct DualOptimizeGuard
    //	{
    //		DualOptimizeGuard( SolverImpl& impl ) : m_impl( impl ) {}
    //		~DualOptimizeGuard() { m_impl.dualOptimize(); }
    //		SolverImpl& m_impl;
    //	};

    //public:
    //private:

    //	SolverImpl( const SolverImpl& );

    //	SolverImpl& operator=( const SolverImpl& );

    //	struct RowDeleter
    //	{
    //		template<typename T>
    //		void operator()( T& pair ) { delete pair.second; }
    //	};

    //}
}