﻿using System;
using System.Collections.Generic;

namespace Bitbucket.Models
{
    public partial class RefreshToken
    {
        public int TokenId { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

        public virtual User User { get; set; }
    }
}
