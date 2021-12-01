/*
    Cassowary.net: an incremental constraint solver for .NET
    (http://lumumba.uhasselt.be/jo/projects/cassowary.net/)
    
    Copyright (C) 2005-2006	Jo Vermeulen (jo.vermeulen@uhasselt.be)
        
    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public License
    as published by the Free Software Foundation; either version 2.1
    of	the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA	 02111-1307, USA.
*/

using System;

namespace CassowaryNET
{
    internal static class MathHelper
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static bool Approx(double a, double b)
        {
            const double epsilon = 1.0e-8;

            if (a == 0d)
            {
                return Math.Abs(b) < epsilon;
            }
            if (b == 0d)
            {
                return Math.Abs(a) < epsilon;
            }
            
            return Math.Abs(a - b) < Math.Abs(a)*epsilon;
        }

        #endregion
    }
}