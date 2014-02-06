//	VoterMart.Geoserver.GeocodeService
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using Manifold.Interop;

namespace VoterMart.Geoserver
{
    /// <summary>
    /// Provides geocoding services for United States addresses only.
    /// </summary>
    [WebService(Description = "Provides geocoding services for United States addresses only.",
        Namespace = "http://votermart.com/geoserver",
        Name = "GeocodeService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class GeocodeService : System.Web.Services.WebService
    {
        #region Private variables

        /// <summary>
        /// Reference to Manifold application object.
        /// </summary>
        private Manifold.Interop.Application _ManifoldApp;

        #endregion

        #region Constructor
        /// <summary>
        /// Initialize instance of GeocodeService object.
        /// </summary>
        public GeocodeService()
        {
            //  Initialize Manifold application
            _ManifoldApp = new ApplicationClass();
        }

        #endregion

        #region Web methods

        /// <summary>
        /// Geocode a raw address.  (United States only.)
        /// </summary>
        /// <param name="address">string: Raw address.</param>
        /// <returns>string: {Status}|{Longitude}|{Latitude}</returns>
        [WebMethod(Description = "Geocode raw addresses for United States only.")]
        public string GeocodeRawAddress(string address)
        {
            string results = "";

            Manifold.Interop.Geocoder geocoder = _ManifoldApp.NewGeocoder();
            Manifold.Interop.GeocodeMatchSet matches = geocoder.LocateAddressRaw(address, 0);
            if (matches.Count == 0)
                results = "No address matches";
            else
                results = System.String.Format("{0}|{1}|{2}", matches[0].StatusText,
                    matches[0].Longitude.ToString(), matches[0].Latitude.ToString());

            return results;
        }

        /// <summary>
        /// Geocode a raw address.  (United States only.)
        /// </summary>
        /// <param name="address">string: Raw address.</param>
        /// <returns>GeoMatch</returns>
        [WebMethod(Description = "Geocode raw addresses for United States only returning a GeoMatch struct.")]
        public GeoMatch GeocodeRawAddressWithStruct(string address)
        {
            GeoMatch geoMatch = new GeoMatch();

            Manifold.Interop.Geocoder geocoder = _ManifoldApp.NewGeocoder();
            Manifold.Interop.GeocodeMatchSet matches = geocoder.LocateAddressRaw(address, 0);
            if (matches.Count == 0)
            {
                geoMatch.StatusText = "No address matches";
            }
            else
            {
                geoMatch.Latitude = matches[0].Latitude;
                geoMatch.Longitude = matches[0].Longitude;
                geoMatch.StatusText = matches[0].StatusText;
                geoMatch.Address = matches[0].Address;
                geoMatch.City = matches[0].City;
                geoMatch.State = matches[0].State;
                geoMatch.Zip = matches[0].Zip;
            }

            return geoMatch;
        }

        /// <summary>
        /// Geocode a standard address. (United States only.)
        /// </summary>
        /// <param name="address">string: Street address.</param>
        /// <param name="city">string: City name.</param>
        /// <param name="state">string: State abbreviation.</param>
        /// <param name="zip">string: Zip code.</param>
        /// <returns>string: {Status}|{Longitude}|{Latitude}</returns>
        [WebMethod(Description = "Geocode standard addresses for United States only.")]
        public string GeocodeAddress(string address, string city, string state, string zip)
        {
            string results = "";

            Manifold.Interop.Geocoder geocoder = _ManifoldApp.NewGeocoder();
            Manifold.Interop.GeocodeMatchSet matches = geocoder.LocateAddress(address,
                city, state, zip, "United States", 0);
            if (matches.Count == 0)
                results = "No address matches";
            else
                results = System.String.Format("{0}|{1}|{2}", matches[0].StatusText,
                    matches[0].Longitude.ToString(), matches[0].Latitude.ToString());

            return results;
        }

        #endregion
    }
}
