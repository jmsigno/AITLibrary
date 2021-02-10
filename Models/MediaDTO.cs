using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AITLibrary.Models
{
    public class MediaDTO
    {
        public MediaDTO(){}

        public MediaDTO(int id, string title, int gen, int dir, int lan, int year, decimal b)
        {
            this.mediaID = id;
            this.mediaTitle = title;
            this.genre = genre;
            this.director = dir;
            this.language = lan;
            this.publishYear = year;
            this.budget = b;
        }

        public MediaDTO(int year)
        {
            this.publishYear = year;
        }

        public int mediaID { get; set; }
        public string mediaTitle { get; set; }
        public int genre { get; set; }
        public int director { get; set; }
        public int language { get; set; }
        public int publishYear { get; set; }
        public decimal budget { get; set; }
    }
}