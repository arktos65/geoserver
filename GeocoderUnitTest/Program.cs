//  GeocoderUnitTest
//	Copyright (c) 2009 by c:Spot InterWorks.  All Rights Reserved.
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
//
//  Modification History:
//  02-MAR-2009     S.M. Sullivan
//                  Created unit test program.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeocoderUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter raw street address:");
            string rawAddress = Console.ReadLine();

            //  Instantiate the geocode server object
            VoterMart.Tools.Geocoder.GeocodeMatch match = new VoterMart.Tools.Geocoder.GeocodeMatch();
            VoterMart.Tools.Geocoder.Server geocoder = new VoterMart.Tools.Geocoder.Server();

            //  Geocode the address
            try
            {
                match = geocoder.GeocodeRawAddress(rawAddress);
                Console.WriteLine("Longitude = " + match.Longitude.ToString());
                Console.WriteLine("Latitude = " + match.Latitude.ToString());
                Console.WriteLine("Status = " + match.StatusText.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
