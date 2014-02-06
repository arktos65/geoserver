//	VoterMart.Geoserver.DelegateStruct
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
    /// Complex type representing legislative delegate information.
    /// </summary>
    public struct DelegateStruct
    {
        #region Private variables
        /// <summary>
        /// Private variables for public accessors.
        /// </summary>
        private string _Status;
        private string _Message;
        private string _Title;
        private string _FirstName;
        private string _MiddleName;
        private string _LastName;
        private string _Suffix;
        private string _Address1;
        private string _Address2;
        private string _City;
        private string _State;
        private string _Zip5;
        private string _Zip4;
        private string _VoiceAreaCode;
        private string _VoicePhone;
        private string _FaxAreaCode;
        private string _FaxPhone;
        private string _Party;
        private string _WebsiteURL;
        private string _Email;
        private LegislativeHouseEnum _LegislativeHouse;
        private string _DistrictNumber;
        #endregion

        #region Accessors
        /// <summary>
        /// Gets or sets the result status code (OK or ERROR).
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
        /// Gets or sets the result error message.
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
        /// Gets or sets the delegate's title.
        /// </summary>
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's first name.
        /// </summary>
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's middle name.
        /// </summary>
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                _MiddleName = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's last name.
        /// </summary>
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's name suffix.
        /// </summary>
        public string Suffix
        {
            get
            {
                return _Suffix;
            }
            set
            {
                _Suffix = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's address.
        /// </summary>
        public string Address1
        {
            get
            {
                return _Address1;
            }
            set
            {
                _Address1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's address.
        /// </summary>
        public string Address2
        {
            get
            {
                return _Address2;
            }
            set
            {
                _Address2 = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's address.
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
        /// Gets or sets the delegate's address.
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
        /// Gets or sets the delegate's address.
        /// </summary>
        public string Zip5
        {
            get
            {
                return _Zip5;
            }
            set
            {
                _Zip5 = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's address.
        /// </summary>
        public string Zip4
        {
            get
            {
                return _Zip4;
            }
            set
            {
                _Zip4 = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's voice phone area code.
        /// </summary>
        public string VoiceAreaCode
        {
            get
            {
                return _VoiceAreaCode;
            }
            set
            {
                _VoiceAreaCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's voice phone number.
        /// </summary>
        public string VoicePhone
        {
            get
            {
                return _VoicePhone;
            }
            set
            {
                _VoicePhone = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's fax area code.
        /// </summary>
        public string FaxAreaCode
        {
            get
            {
                return _FaxAreaCode;
            }
            set
            {
                _FaxAreaCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's fax phone number.
        /// </summary>
        public string FaxPhone
        {
            get
            {
                return _FaxPhone;
            }
            set
            {
                _FaxPhone = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's political party code.
        /// </summary>
        public string Party
        {
            get
            {
                return _Party;
            }
            set
            {
                _Party = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's website URL.
        /// </summary>
        public string WebsiteURL
        {
            get
            {
                return _WebsiteURL;
            }
            set
            {
                _WebsiteURL = value;
            }
        }

        /// <summary>
        /// Gets or sets the delegate's email address.
        /// </summary>
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }

        /// <summary>
        /// Gets or sets the legislative house code.
        /// </summary>
        public LegislativeHouseEnum LegislativeHouse
        {
            get
            {
                return _LegislativeHouse;
            }
            set
            {
                _LegislativeHouse = value;
            }
        }

        /// <summary>
        /// Gets or sets the legislative district number.
        /// </summary>
        public string DistrictNumber
        {
            get
            {
                return _DistrictNumber;
            }
            set
            {
                _DistrictNumber = value;
            }
        }
        #endregion
    }
}
