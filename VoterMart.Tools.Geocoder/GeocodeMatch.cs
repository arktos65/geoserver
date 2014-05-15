//	VoterMart.Tools.Geocoder
//	Copyright (c) 2009-2014 by Sean M. Sullivan.  All Rights Reserved.
//
//	The above copyright notice and this permission notice shall be included in all 
//	copies or substantial portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//	PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
//	FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//	ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
//  IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoterMart.Tools.Geocoder
{
    /// <summary>
    /// Geocoder match result data structure.
    /// </summary>
    public struct GeocodeMatch
    {
        #region Private variables
        /// <summary>
        /// StatusText property.
        /// </summary>
        private string _StatusText;
        /// <summary>
        /// Longitude property.
        /// </summary>
        private double _Longitude;
        /// <summary>
        /// Latitude property.
        /// </summary>
        private double _Latitude;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets or sets the geocoder result status text.
        /// </summary>
        public string StatusText
        {
            get
            {
                return _StatusText;
            }
            set
            {
                _StatusText = value;
            }
        }

        /// <summary>
        /// Gets or sets the longitude result.
        /// </summary>
        public double Longitude
        {
            get
            {
                return _Longitude;
            }
            set
            {
                _Longitude = value;
            }
        }

        /// <summary>
        /// Gets or sets the latitude result.
        /// </summary>
        public double Latitude
        {
            get
            {
                return _Latitude;
            }
            set
            {
                _Latitude = value;
            }
        }
        #endregion
    }
}
