using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiwi_Ts
{
    /**
     * The primary user constraint variable.
     *
     * @class
     * @param {String} [name=""] The name to associated with the variable.
     */
    public partial class Variable:IKeyId
    {
        
        public Variable(string name = "")
        {
            Name = name;
            Id = VarId++;
        }

       
        /**
         * Returns the JSON representation of the variable.
         * @private
         */
        public string toJSON()
        {
            throw new NotImplementedException();
        }

        public string ToString()
        {
            return this.Context + "[" + this.Name + ":" + this.Value + "]";
        }

        public int id() => Id;

        /// <summary>
        /// Returns the name of the variable
        /// </summary>
        public string Name { get; set; }
        public double Value { get; set; }
        public object Context=null;
        /// <summary>
        /// Returns the unique id number of the variable.
        /// </summary>
        public int Id { get; private set; }

        ///**
        // * The internal variable id counter.
        // * @private
        // */
        // let VarId = 0;
        static int VarId = 0;
    }

}
