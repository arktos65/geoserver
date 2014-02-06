//	VoterMart.Geoserver.DistrictMatch
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
    /// Complex type that describes a district match.
    /// </summary>
    public struct DistrictMatch
    {
        #region Private variables
        /// <summary>
        /// Private fields for public accessors
        /// </summary>
        private string _Status;
        private string _Message;
        private string _District;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }

        /// <summary>
        /// Gets or sets the district number.
        /// </summary>
        public string District
        {
            get
            {
                return _District;
            }
            set
            {
                _District = value;
            }
        }
        #endregion
    }
}
