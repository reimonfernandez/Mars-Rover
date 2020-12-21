using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MarsRover.Service.Contracts
{
    public interface IMarsRoverService
    {
        HttpResponseMessage GetPhotosByDate(string date);
        List<string> ExtractTextLinesFromFile(System.Web.HttpPostedFileBase filePath);
        List<DateTime> GetAllValidDates(List<string> linesOfText);
    }
}
