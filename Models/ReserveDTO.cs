using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AITLibrary.Models
{
    public class ReserveDTO
    {
        public ReserveDTO() { }

        public ReserveDTO(int rid, int uid, int mediaID, DateTime rd)
        {
            this.RID = rid;
            this.UID = uid;
            this.MediaID = mediaID;
            this.ReservedDate = rd;
        }

        public int RID { get; set; }
        public int UID { get; set; }
        public int MediaID { get; set; }
        public DateTime ReservedDate { get; set; }
    }
}