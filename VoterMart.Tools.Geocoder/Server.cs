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

using Manifold.Interop;

namespace VoterMart.Tools.Geocoder
{
    /// <summary>
    /// Represents Manifold GIS geocoding server object.
    /// </summary>
    public class Server
    {
        #region Private variables
        /// <summary>
        /// ManifoldApp property.
        /// </summary>
        private Manifold.Interop.Application _ManifoldApp;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize instance of geocoding server object.
        /// </summary>
        public Server()
        {
            //  Initialize instance of Manifold runtime object
            _ManifoldApp = new Application();
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Gets the instance of Manifold application runtime object.
        /// </summary>
        public Manifold.Interop.Application ManifoldApp
        {
            get
            {
                return _ManifoldApp;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Return the geocoordinates of the provided raw address.
        /// </summary>
        /// <param name="address">string: Raw address.</param>
        /// <returns>VoterMart.Tools.Geocoder.GeocodeMatch</returns>
        public GeocodeMatch GeocodeRawAddress(string address)
        {
            //  Initialize instance of geocoder
            GeocodeMatch geomatch = new GeocodeMatch();
            Manifold.Interop.Geocoder geocoder = this.ManifoldApp.NewGeocoder();

            //  Geocode the raw address and return a match set.
            Manifold.Interop.GeocodeMatchSet matches = geocoder.LocateAddressRaw(address, 0);
            if (matches.Count == 0)
            {
                //  No match found
                geomatch.StatusText = "No address matches";
                geomatch.Latitude = 0;
                geomatch.Longitude = 0;
            }
            else
            {
                //  Match found
                geomatch.StatusText = "OK";
                geomatch.Latitude = matches[0].Latitude;
                geomatch.Longitude = matches[0].Longitude;
            }
            return geomatch;
        }
        #endregion
    }
}
