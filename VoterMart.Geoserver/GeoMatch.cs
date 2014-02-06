//	VoterMart.Geoserver.GeoMatch
//	Copyright (c) 2009-2011 by c.Spot InterWorks.  All Rights Reserved.
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
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace VoterMart.Geoserver
{
    /// <summary>
    /// Complex type that describes the results of a geocoder match.
    /// </summary>
    public struct GeoMatch
    {
        #region Private variables
        /// <summary>
        /// Longitude property.
        /// </summary>
        private double _Longitude;
        /// <summary>
        /// Latitude property.
        /// </summary>
        private double _Latitude;
        /// <summary>
        /// Address property.
        /// </summary>
        private string _Address;
        /// <summary>
        /// City property.
        /// </summary>
        private string _City;
        /// <summary>
        /// State property.
        /// </summary>
        private string _State;
        /// <summary>
        /// Zip property.
        /// </summary>
        private string _Zip;
        /// <summary>
        /// StatusText property.
        /// </summary>
        private string _StatusText;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets or sets the longitude value.
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
        /// Gets or sets the latitude value.
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

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
            }
        }

        /// <summary>
        /// Gets or sets the city name.
        /// </summary>
        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                _City = value;
            }
        }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        public string State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
            }
        }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        public string Zip
        {
            get
            {
                return _Zip;
            }
            set
            {
                _Zip = value;
            }
        }

        /// <summary>
        /// Gets or sets the status text.
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
        #endregion
    }
}
