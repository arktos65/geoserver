using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace VoterMart.Geoserver
{
    /// <summary>
    /// Enumeration of legislative house types.
    /// </summary>
    public enum LegislativeHouseEnum
    {
        LOWER,
        UPPER,
        CONGRESS,
        SENATE
    }
}
