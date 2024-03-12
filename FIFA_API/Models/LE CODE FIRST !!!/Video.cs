using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_video_vid")]
    public class Video
    {
		[Column("vid_nom")]
        public string Nom { get; set; }

		[Column("vid_url")]
        public string Url { get; set; }

    }
}
