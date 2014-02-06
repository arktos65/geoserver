//	VoterMart.Geoserver.DistrictCodeService
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
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using Manifold.Interop;

namespace VoterMart.Geoserver
{
    /// <summary>
    /// Provides services for looking up legislative districts based on street address.
    /// </summary>
    [WebService(Description = "Provides services for looking up legislative districts based on street address.  See release notes for additional information.",
        Namespace = "http://votermart.com/geoserver",
        Name = "DistrictCodeService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class DistrictCodeService : System.Web.Services.WebService
    {
        #region Constants
        /// <summary>
        /// Lower House query template.
        /// </summary>
        private const string LOWHOUSE_QUERY = "SELECT [{0}-LowerDrawing].[DISTRICT] FROM [{0}-LowerDrawing] " +
            "WHERE Contains([{0}-LowerDrawing].[ID],NewPointLatLon({1},{2}))";
        /// <summary>
        /// Upper House query template.
        /// </summary>
        private const string UPHOUSE_QUERY = "SELECT [{0}-UpperDrawing].[DISTRICT] FROM [{0}-UpperDrawing] " +
            "WHERE Contains([{0}-UpperDrawing].[ID],NewPointLatLon({1},{2}))";
        /// <summary>
        /// House of Representatives query template.
        /// </summary>
        private const string CONGRESS_QUERY = "SELECT [{0}-CongressDrawing].[DISTRICT] FROM [{0}-CongressDrawing] " +
            "WHERE Contains([{0}-CongressDrawing].[ID],NewPointLatLon({1},{2}))";
        /// <summary>
        /// Lower House Delegate query template.  {0} = 5-digit FIPS.  {1} = District number.
        /// </summary>
        private const string LOWDELEG_QUERY = "SELECT * FROM [{0}-Legislators] " +
            "WHERE ([{0}-Legislators].[LEGTYPE]=\"LOWER\" AND [{0}-Legislators].[DIST_NBR]=\"{1}\")";
        /// <summary>
        /// Upper House Delegate query template.  {0} = 5-digit FIPS.  {1} = District number.
        /// </summary>
        private const string UPDELEG_QUERY = "SELECT * FROM [{0}-Legislators] " +
            "WHERE ([{0}-Legislators].[LEGTYPE]=\"UPPER\" AND [{0}-Legislators].[DIST_NBR]=\"{1}\")";
        /// <summary>
        /// Congressional Delegate query template.  {0} = 5-digit FIPS.  {1} = District number.
        /// </summary>
        private const string CONGDELEG_QUERY = "SELECT * FROM [{0}-Legislators] " +
            "WHERE ([{0}-Legislators].[LEGTYPE]=\"CONGRESS\" AND [{0}-Legislators].[DIST_NBR]=\"{1}\")";
        /// <summary>
        /// Number of columns to use in Legislator table.
        /// </summary>
        private const int LEGCOLUMN_COUNT = 20;
        #endregion

        #region Fields
        /// <summary>
        /// Reference to Manifold application objects.
        /// </summary>
        private Manifold.Interop.Application _ManifoldApp;
        private Manifold.Interop.Document _ManifoldDoc;
        private Manifold.Interop.ComponentSet _ManifoldCompset;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize instance of GeocodeService object.
        /// </summary>
        public DistrictCodeService()
        {
            //  Initialize Manifold application
            _ManifoldApp = new ApplicationClass();
            _ManifoldDoc = _ManifoldApp.DocumentSet.Open(Server.MapPath("~/App_Data/Legislative-Geodb.map").ToString(), false);
            _ManifoldCompset = _ManifoldDoc.ComponentSet;
        }

        #endregion

        #region Web methods

        /// <summary>
        /// Lookup congressional district number based on address.
        /// </summary>
        /// <param name="address">string: Raw street address.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>STATUS CODE|DISTRICT</returns>
        [WebMethod(Description = "Returns congressional district number based on raw street address.")]
        public string GetCongressDistrict(string address, string fips)
        {
            StringBuilder result = new StringBuilder();

            //  First geocode the raw address to get geocoordinates
            Manifold.Interop.GeocodeMatchSet matchset = this.GeocodeRawAddress(address);
            if (matchset.Count == 0)
            {
                result.Append("ERROR|No address matches");
            }
            else
            {
                result.Append(this.LookupCongress(matchset[0].Longitude, matchset[0].Latitude, fips));
            }

            return result.ToString();
        }

        /// <summary>
        /// Lookup congressional delegate contact information based on address.
        /// </summary>
        /// <param name="address">string: Raw street address.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>DelegateStruct: object.</returns>
        [WebMethod(Description = "Returns congressional delegate based on raw street address.")]
        public DelegateStruct GetCongressDelegate(string address, string fips)
        {
            DelegateStruct delegateResult = new DelegateStruct();
            string district = String.Empty;

            //  Geocode the raw address to get spatial coordinates and district number
            Manifold.Interop.GeocodeMatchSet matchset = this.GeocodeRawAddress(address);
            if (matchset.Count == 0)
            {
                //  No address found, so set error status with message
                delegateResult.Status = "ERROR";
                delegateResult.Message = "No address match found.";
            }
            else
            {
                //  Lookup the lower house district number for spatial coordinates
                district = this.LookupCongress(matchset[0].Longitude, matchset[0].Latitude, fips);
                //  Check status, if OK then lookup delegate by passing second string
                //  in district variable.
                if (district.Split('|')[0] == "OK")
                    delegateResult = this.LookupDelegate(LegislativeHouseEnum.CONGRESS, district.Split('|')[1], fips);
                else
                {
                    //  Set the error status with message
                    delegateResult.Status = "ERROR";
                    delegateResult.Message = district.Split('|')[1];
                }
            }

            return delegateResult;
        }

        /// <summary>
        /// Lookup lower state house district number based on address.
        /// </summary>
        /// <param name="address">string: Raw street address.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>STATUS CODE|DISTRICT</returns>
        [WebMethod(Description = "Returns lower state house district number based on raw street address.")]
        public string GetLowerHouseDistrict(string address, string fips)
        {
            StringBuilder result = new StringBuilder();

            //  First geocode the raw address to get geocoordinates
            Manifold.Interop.GeocodeMatchSet matchset = this.GeocodeRawAddress(address);
            if (matchset.Count == 0)
            {
                result.Append("ERROR|No address matches");
            }
            else
            {
                result.Append(this.LookupLower(matchset[0].Longitude, matchset[0].Latitude, fips));
            }

            return result.ToString();
        }

        /// <summary>
        /// Lookup lower house delegate contact information based on address.
        /// </summary>
        /// <param name="address">string: Raw street address.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>DelegateStruct: object.</returns>
        [WebMethod(Description = "Returns lower house delegate based on raw street address.")]
        public DelegateStruct GetLowerHouseDelegate(string address, string fips)
        {
            DelegateStruct delegateResult = new DelegateStruct();
            string district = String.Empty;

            //  Geocode the raw address to get spatial coordinates and district number
            Manifold.Interop.GeocodeMatchSet matchset = this.GeocodeRawAddress(address);
            if (matchset.Count == 0)
            {
                //  No address found, so set error status with message
                delegateResult.Status = "ERROR";
                delegateResult.Message = "No address match found.";
            }
            else
            {
                //  Lookup the lower house district number for spatial coordinates
                district = this.LookupLower(matchset[0].Longitude, matchset[0].Latitude, fips);
                //  Check status, if OK then lookup delegate by passing second string
                //  in district variable.
                if (district.Split('|')[0] == "OK")
                    delegateResult = this.LookupDelegate(LegislativeHouseEnum.LOWER, district.Split('|')[1], fips);
                else
                {
                    //  Set the error status with message
                    delegateResult.Status = "ERROR";
                    delegateResult.Message = district.Split('|')[1];
                }
            }

            return delegateResult;
        }

        /// <summary>
        /// Lookup upper state house district number based on address.
        /// </summary>
        /// <param name="address">string: Raw street address.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>STATUS CODE|DISTRICT</returns>
        [WebMethod(Description = "Returns upper state house district number based on raw street address.")]
        public string GetUpperHouseDistrict(string address, string fips)
        {
            StringBuilder result = new StringBuilder();

            //  First geocode the raw address to get geocoordinates
            Manifold.Interop.GeocodeMatchSet matchset = this.GeocodeRawAddress(address);
            if (matchset.Count == 0)
            {
                result.Append("ERROR|No address matches");
            }
            else
            {
                result.Append(this.LookupUpper(matchset[0].Longitude, matchset[0].Latitude, fips));
            }

            return result.ToString();
        }

        /// <summary>
        /// Lookup upper house delegate contact information based on address.
        /// </summary>
        /// <param name="address">string: Raw street address.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>DelegateStruct: object.</returns>
        [WebMethod(Description = "Returns upper house delegate based on raw street address.")]
        public DelegateStruct GetUpperHouseDelegate(string address, string fips)
        {
            DelegateStruct delegateResult = new DelegateStruct();
            string district = String.Empty;

            //  Geocode the raw address to get spatial coordinates and district number
            Manifold.Interop.GeocodeMatchSet matchset = this.GeocodeRawAddress(address);
            if (matchset.Count == 0)
            {
                //  No address found, so set error status with message
                delegateResult.Status = "ERROR";
                delegateResult.Message = "No address match found.";
            }
            else
            {
                //  Lookup the lower house district number for spatial coordinates
                district = this.LookupUpper(matchset[0].Longitude, matchset[0].Latitude, fips);
                //  Check status, if OK then lookup delegate by passing second string
                //  in district variable.
                if (district.Split('|')[0] == "OK")
                    delegateResult = this.LookupDelegate(LegislativeHouseEnum.UPPER, district.Split('|')[1], fips);
                else
                {
                    //  Set the error status with message
                    delegateResult.Status = "ERROR";
                    delegateResult.Message = district.Split('|')[1];
                }
            }

            return delegateResult;
        }

        /// <summary>
        /// Lookup congressional, lower, and upper house district numbers based on address.
        /// </summary>
        /// <param name="address">string: Raw street address.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>STATUS CODE|CONGRESS|STATUS CODE|UPPER HOUSE|STATUS CODE|LOWER HOUSE</returns>
        [WebMethod(Description = "Returns congressional, lower state house, and upper state house district numbers based on raw street address.")]
        public string GetLegislativeDistricts(string address, string fips)
        {
            bool errorStatus = false;
            StringBuilder result = new StringBuilder();

            //  First geocode the raw address to get geocoordinates
            Manifold.Interop.GeocodeMatchSet matchset = this.GeocodeRawAddress(address);
            if (matchset.Count == 0)
            {
                result.Append("ERROR|No address matches");
                errorStatus = true;
            }

            //  If no error condition exists, perform the district lookups
            if (!errorStatus)
            {
                //  Lookup the congression district
                result.Append(this.LookupCongress(matchset[0].Longitude, matchset[0].Latitude, fips) + "|");

                //  Lookup the lower house district
                result.Append(this.LookupLower(matchset[0].Longitude, matchset[0].Latitude, fips) + "|");

                //  Lookup the upper house district
                result.Append(this.LookupUpper(matchset[0].Longitude, matchset[0].Latitude, fips));
            }

            return result.ToString();
        }

        /// <summary>
        /// Lookup both state house delegate's contact information based on address.
        /// </summary>
        /// <param name="address">string: Raw street address.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>DelegateStruct[]: object array.</returns>
        [WebMethod(Description = "Returns upper house delegate based on raw street address.")]
        public DelegateStruct[] GetStateDelegates(string address, string fips)
        {
            DelegateStruct[] delegateResult = new DelegateStruct[2];
            string lower = String.Empty;
            string upper = String.Empty;

            //  Geocode the raw address to get spatial coordinates and district number
            Manifold.Interop.GeocodeMatchSet matchset = this.GeocodeRawAddress(address);
            if (matchset.Count == 0)
            {
                //  No address found, so set error status with message
                delegateResult[0].Status = "ERROR";
                delegateResult[0].Message = "No address match found.";
            }
            else
            {
                //  Lookup the lower house district number for spatial coordinates
                lower = this.LookupLower(matchset[0].Longitude, matchset[0].Latitude, fips);
                if (lower.Split('|')[0] == "OK")
                    delegateResult[0] = this.LookupDelegate(LegislativeHouseEnum.LOWER, lower.Split('|')[1], fips);
                else
                {
                    //  Set the error status with message
                    delegateResult[0].Status = "ERROR";
                    delegateResult[0].Message = lower.Split('|')[1];
                }
                //  Lookup the upper house district number for spatial coordinates
                upper = this.LookupUpper(matchset[0].Longitude, matchset[0].Latitude, fips);
                if (upper.Split('|')[0] == "OK")
                    delegateResult[1] = this.LookupDelegate(LegislativeHouseEnum.UPPER, upper.Split('|')[1], fips);
                else
                {
                    //  Set the error status with message
                    delegateResult[1].Status = "ERROR";
                    delegateResult[1].Message = upper.Split('|')[1];
                }
            }

            return delegateResult;
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Geocode the raw address and return geocoordinates.
        /// </summary>
        /// <param name="address">string: Raw address.</param>
        /// <returns>STATUS CODE|LONGITUDE|LATITUDE</returns>
        private Manifold.Interop.GeocodeMatchSet GeocodeRawAddress(string address)
        {
            //  Initialize geocoder and locate address
            Manifold.Interop.Geocoder geocoder = _ManifoldApp.NewGeocoder();
            Manifold.Interop.GeocodeMatchSet matches = geocoder.LocateAddressRaw(address, 0);

            return matches;
        }

        /// <summary>
        /// Lookup the lower state house district number for the specified geocoordinates.
        /// </summary>
        /// <param name="longitude">double: Longitude.</param>
        /// <param name="latitude">double: Latitude.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>STATUS CODE|DISTRICT NUMBER</returns>
        private string LookupLower(double longitude, double latitude, string fips)
        {
            bool errorStatus = false;
            string result = "";

            //  Check the coordinates to ensure they're within the expected range
            if (longitude < -180 || longitude > 180)
            {
                result = "ERROR|Invalid longitude value";
                errorStatus = true;
            }
            if (latitude < -90 || latitude > 90)
            {
                result = "ERROR|Invalid latitude value";
                errorStatus = true;
            }

            //  If no error exists, then process the query and return the results
            if (!errorStatus)
            {
                //  Create the query object and specify the query to be used against the 
                //  drawing in the Manifold document.
                Manifold.Interop.Query query = _ManifoldDoc.NewQuery("TempLowerQuery", true);
                query.Text = System.String.Format(LOWHOUSE_QUERY, fips, longitude.ToString(), 
                    latitude.ToString());
                query.Run();

                //  Now get the query results and determine if any records were returned
                if (query.Table.RecordSet.Count == 0)
                {
                    result = "ERROR|No district match";
                }
                else
                {
                    Manifold.Interop.RecordSet rs = query.Table.RecordSet;
                    result = "OK|" + rs[0]._GetDataByIndex(0).ToString();
                }

                //  Remove the query from the Manifold document
                _ManifoldCompset.Remove("TempLowerQuery");
            }
            return result;
        }

        /// <summary>
        /// Lookup the upper state house district number for the specified geocoordinates.
        /// </summary>
        /// <param name="longitude">double: Longitude.</param>
        /// <param name="latitude">double: Latitude.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>STATUS CODE|DISTRICT NUMBER</returns>
        private string LookupUpper(double longitude, double latitude, string fips)
        {
            bool errorStatus = false;
            string result = "";

            //  Check the coordinates to ensure they're within the expected range
            if (longitude < -180 || longitude > 180)
            {
                result = "ERROR|Invalid longitude value";
                errorStatus = true;
            }
            if (latitude < -90 || latitude > 90)
            {
                result = "ERROR|Invalid latitude value";
                errorStatus = true;
            }

            //  If no error exists, then process the query and return the results
            if (!errorStatus)
            {
                //  Create the query object and specify the query to be used against the 
                //  drawing in the Manifold document.
                Manifold.Interop.Query query = _ManifoldDoc.NewQuery("TempUpperQuery", true);
                query.Text = System.String.Format(UPHOUSE_QUERY, fips, longitude.ToString(),
                    latitude.ToString());
                query.Run();

                //  Now get the query results and determine if any records were returned
                if (query.Table.RecordSet.Count == 0)
                {
                    result = "ERROR|No district match";
                }
                else
                {
                    Manifold.Interop.RecordSet rs = query.Table.RecordSet;
                    result = "OK|" + rs[0]._GetDataByIndex(0).ToString();
                }

                //  Remove the query from the Manifold document
                _ManifoldCompset.Remove("TempUpperQuery");
            }
            return result;
        }

        /// <summary>
        /// Lookup the congressional district number for the specified geocoordinates.
        /// </summary>
        /// <param name="longitude">double: Longitude.</param>
        /// <param name="latitude">double: Latitude.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>STATUS CODE|DISTRICT NUMBER</returns>
        private string LookupCongress(double longitude, double latitude, string fips)
        {
            bool errorStatus = false;
            string result = "";

            //  Check the coordinates to ensure they're within the expected range
            if (longitude < -180 || longitude > 180)
            {
                result = "ERROR|Invalid longitude value";
                errorStatus = true;
            }
            if (latitude < -90 || latitude > 90)
            {
                result = "ERROR|Invalid latitude value";
                errorStatus = true;
            }

            //  If no error exists, then process the query and return the results
            if (!errorStatus)
            {
                //  Create the query object and specify the query to be used against the 
                //  drawing in the Manifold document.
                Manifold.Interop.Query query = _ManifoldDoc.NewQuery("TempCongQuery", true);
                query.Text = System.String.Format(CONGRESS_QUERY, fips, longitude.ToString(),
                    latitude.ToString());
                query.Run();

                //  Now get the query results and determine if any records were returned
                if (query.Table.RecordSet.Count == 0)
                {
                    result = "ERROR|No district match";
                }
                else
                {
                    Manifold.Interop.RecordSet rs = query.Table.RecordSet;
                    result = "OK|" + rs[0]._GetDataByIndex(0).ToString();
                }

                //  Remove the query from the Manifold document
                _ManifoldCompset.Remove("TempCongQuery");
            }
            return result;
        }

        /// <summary>
        /// Lookup legislative delegate contact information based on geocoordinates.
        /// </summary>
        /// <param name="legtype">LegislativeHouseEnum: enumeration</param>
        /// <param name="district">string: District number.</param>
        /// <param name="fips">string: State FIPS code.</param>
        /// <returns>DelegateStruct: object.</returns>
        private DelegateStruct LookupDelegate(LegislativeHouseEnum legtype, string district, string fips)
        {
            bool errorStatus = false;
            StringBuilder result = new StringBuilder();
            DelegateStruct ds = new DelegateStruct();
            ds.LegislativeHouse = legtype;

            //  Check the coordinates to ensure they're within the expected range
            if (district.Length == 0)
            {
                ds.Status = "ERROR";
                ds.Message = "The district number provided is invalid.";
                errorStatus = true;
            }

            //  If no error exists, then process the query and return the results
            if (!errorStatus)
            {
                //  Create the query object and specify the query to be used against the 
                //  drawing in the Manifold document.
                Manifold.Interop.Query query = _ManifoldDoc.NewQuery("TempDelegQuery", true);
                switch (legtype)
                {
                    case LegislativeHouseEnum.CONGRESS:
                        query.Text = System.String.Format(CONGDELEG_QUERY, fips, district);
                        break;
                    case LegislativeHouseEnum.LOWER:
                        query.Text = System.String.Format(LOWDELEG_QUERY, fips, district);
                        break;
                    case LegislativeHouseEnum.UPPER:
                        query.Text = System.String.Format(UPDELEG_QUERY, fips, district);
                        break;
                }
                query.Run();

                //  Now get the query results and determine if any records were returned
                if (query.Table.RecordSet.Count == 0)
                {
                    ds.Status = "ERROR";
                    ds.Message = "No district match was found.";
                }
                else
                {
                    ds = ConvertDelegateToStruct(query.Table.RecordSet);
                }

                //  Remove the query from the Manifold document
                _ManifoldCompset.Remove("TempDelegQuery");
            }
            return ds;
        }

        /// <summary>
        /// Converts a Manifold RecordSet Legislators table object to a DelegateStruct object.
        /// </summary>
        /// <param name="rs">Manifold.Interop.RecordSet: object.</param>
        /// <returns>DelegateStruct: object.</returns>
        private DelegateStruct ConvertDelegateToStruct(Manifold.Interop.RecordSet rs)
        {
            DelegateStruct ds = new DelegateStruct();
            ds.Status = "OK";
            ds.Message = String.Empty;
            ds.DistrictNumber = rs[0]._GetData("DIST_NBR").ToString();
            ds.Title = rs[0]._GetData("PREFIX").ToString();
            ds.FirstName = rs[0]._GetData("FNAME").ToString();
            ds.MiddleName = rs[0]._GetData("MNAME").ToString();
            ds.LastName = rs[0]._GetData("LNAME").ToString();
            ds.Suffix = rs[0]._GetData("SUFFIX").ToString();
            ds.Address1 = rs[0]._GetData("ADDRESS1").ToString();
            ds.Address2 = rs[0]._GetData("ADDRESS2").ToString();
            ds.City = rs[0]._GetData("CITY1").ToString();
            ds.State = rs[0]._GetData("STATE1").ToString();
            ds.Zip5 = rs[0]._GetData("ZIP5").ToString();
            ds.Zip4 = rs[0]._GetData("ZIP4").ToString();
            ds.VoiceAreaCode = rs[0]._GetData("WAREACODE").ToString();
            ds.VoicePhone = rs[0]._GetData("WPHONE").ToString();
            ds.FaxAreaCode = rs[0]._GetData("FAREACODE").ToString();
            ds.FaxPhone = rs[0]._GetData("FAXNUMBER").ToString();
            ds.Party = rs[0]._GetData("POL_PARTY").ToString();
            ds.WebsiteURL = rs[0]._GetData("URL").ToString();
            ds.Email = rs[0]._GetData("EMAIL").ToString();
            switch (rs[0]._GetData("LEGTYPE").ToString())
            { 
                case "LOWER":
                    ds.LegislativeHouse = LegislativeHouseEnum.LOWER;
                    break;
                case "UPPER":
                    ds.LegislativeHouse = LegislativeHouseEnum.UPPER;
                    break;
                case "CONGRESS":
                    ds.LegislativeHouse = LegislativeHouseEnum.CONGRESS;
                    break;
                case "SENATE":
                    ds.LegislativeHouse = LegislativeHouseEnum.SENATE;
                    break;
            }
            return ds;
        }
        #endregion
    }
}
