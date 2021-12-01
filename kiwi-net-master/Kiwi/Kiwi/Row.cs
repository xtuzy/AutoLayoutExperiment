using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kiwi
{
    internal class Row
    {

        public Row(double constant = 0.0)
        {
            Cells = new Dictionary<Symbol, double>();
            Constant = constant;
        }

        public Row(Row other)
        {
            Cells = new Dictionary<Symbol, double>(other.Cells);
            Constant = other.Constant;
        }


        public Dictionary<Symbol, double> Cells { get; }
        public double Constant { get; private set; }


        // TODO move away
        public static bool nearZero(double value)
        {
            const double eps = 1.0e-8;
            return value < 0.0 ? -value < eps : value < eps;
        }


        // Add a constant value to the row constant.
        //
        // The new value of the constant is returned.
        public double Add(double value)
        {
            return Constant += value;
        }
        

        // Insert a symbol into the row with a given coefficient.
        //
        // If the symbol already exists in the row, the coefficient will be
        // added to the existing coefficient. If the resulting coefficient
        // is zero, the symbol will be removed from the row.
        public void Insert(Symbol symbol, double coefficient = 1.0)
        {
            coefficient += Cells.GetValueOrDefault(symbol);

            if (nearZero(coefficient))
            {
                Cells.Remove(symbol);
            }
            else
            {
                Cells[symbol] = coefficient;
            }
        }
        //	void insert( const Symbol& symbol, double coefficient = 1.0 )
        //	{
        //		if( nearZero( m_cells[ symbol ] += coefficient ) )
        //			m_cells.erase( symbol );
        //	}


        // Insert a row into this row with a given coefficient.
        //
        // The constant and the cells of the other row will be multiplied by
        // the coefficient and added to this row. Any cell with a resulting
        // coefficient of zero will be removed from the row.
        public void Insert(Row other, double coefficient = 1.0)
        {
            Constant += other.Constant * coefficient;
            foreach (var symbol in other.Cells.Keys)
            {
                Insert(symbol, other.Cells[symbol] * coefficient);
            }
        }
        //	void insert( const Row& other, double coefficient = 1.0 )
        //	{
        //		typedef CellMap::const_iterator iter_t;
        //		m_constant += other.m_constant * coefficient;
        //		iter_t end = other.m_cells.end();
        //		for( iter_t it = other.m_cells.begin(); it != end; ++it )
        //		{
        //			double coeff = it->second * coefficient;
        //			if( nearZero( m_cells[ it->first ] += coeff ) )
        //				m_cells.erase( it->first );
        //		}
        //	}

        
        // Remove the given symbol from the row.
        public void Remove(Symbol symbol)
        {
            Cells.Remove(symbol);
        }
        //	void remove( const Symbol& symbol )
        //	{
        //		CellMap::iterator it = m_cells.find( symbol );
        //		if( it != m_cells.end() )
        //			m_cells.erase( it );
        //	}


        // Reverse the sign of the constant and all cells in the row.
        public void ReverseSign()
        {
            Constant = -Constant;
            foreach (var symbol in Cells.Keys.ToList())
            {
                Cells[symbol] = -Cells[symbol];
            }
        }
        //	void reverseSign()
        //	{
        //		typedef CellMap::iterator iter_t;
        //		m_constant = -m_constant;
        //		iter_t end = m_cells.end();
        //		for( iter_t it = m_cells.begin(); it != end; ++it )
        //			it->second = -it->second;
        //	}


        // Solve the row for the given symbol.
        //
        // This method assumes the row is of the form a * x + b * y + c = 0
        // and (assuming solve for x) will modify the row to represent the
        // right hand side of x = -b/a * y - c / a. The target symbol will
        // be removed from the row, and the constant and other cells will
        // be multiplied by the negative inverse of the target coefficient.
        //
        // The given symbol *must* exist in the row.
        public void SolveFor(Symbol symbol)
        {
            var coeff = -1.0 / Cells[symbol];
            Cells.Remove(symbol);
            Constant *= coeff;
            foreach (var sym in Cells.Keys.ToList())
            {
                Cells[sym] *= coeff;
            }
        }
        //	void solveFor( const Symbol& symbol )
        //	{
        //		typedef CellMap::iterator iter_t;
        //		double coeff = -1.0 / m_cells[ symbol ];
        //		m_cells.erase( symbol );
        //		m_constant *= coeff;
        //		iter_t end = m_cells.end();
        //		for( iter_t it = m_cells.begin(); it != end; ++it )
        //			it->second *= coeff;
        //	}


        // Solve the row for the given symbols.
        //
        // This method assumes the row is of the form x = b * y + c and will
        // solve the row such that y = x / b - c / b. The rhs symbol will be
        // removed from the row, the lhs added, and the result divided by the
        // negative inverse of the rhs coefficient.
        //
        // The lhs symbol *must not* exist in the row, and the rhs symbol
        // *must* exist in the row.
        public void SolveFor(Symbol lhs, Symbol rhs)
        {
            Insert(lhs, -1);
            SolveFor(rhs);
        }
        //	void solveFor( const Symbol& lhs, const Symbol& rhs )
        //	{
        //		insert( lhs, -1.0 );
        //		solveFor( rhs );
        //	}

        
        // Get the coefficient for the given symbol.
        //
        // If the symbol does not exist in the row, zero will be returned.
        public double CoefficientFor(Symbol symbol)
        {
            return Cells.GetValueOrDefault(symbol);
        }
        //	double coefficientFor( const Symbol& symbol ) const
        //	{
        //		CellMap::const_iterator it = m_cells.find( symbol );
        //		if( it == m_cells.end() )
        //			return 0.0;
        //		return it->second;
        //	}
        
        
        // Substitute a symbol with the data from another row.
        //
        // Given a row of the form a * x + b and a substitution of the
        // form x = 3 * y + c the row will be updated to reflect the
        // expression 3 * a * y + a * c + b.
        //
        // If the symbol does not exist in the row, this is a no-op.
        public void Substitude(Symbol symbol, Row row)
        {
            if (Cells.TryGetValue(symbol, out var coefficient))
            {
                Cells.Remove(symbol);
                Insert(row, coefficient);
            }
        }
        //	void substitute( const Symbol& symbol, const Row& row )
        //	{
        //		typedef CellMap::iterator iter_t;
        //		iter_t it = m_cells.find( symbol );
        //		if( it != m_cells.end() )
        //		{
        //			double coefficient = it->second;
        //			m_cells.erase( it );
        //			insert( row, coefficient );
        //		}
        //	}
    }
}