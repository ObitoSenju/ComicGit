using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace xkcdProject.Models
{
    public class Comic
    {
        [Key]
        public int IdComic { get; set; }
        [StringLength(2)]
        public string Day { get; set; }
        [StringLength(2)]
        public string Month { get; set; }
        [StringLength(4)]
        public string Year { get; set; }
        public int Num { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Safe_Title { get; set; }
        public string News { get; set; }
        public string Transcript { get; set; }
        public string Img { get; set; }
        public string Alt { get; set; }
    }
}
