using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.ViewModels.Terminal
{
    public class TerminalStatusViewModel
    {
        public string TrackId { get; set; }

        public long StatusId { get; set; }

        public string StatusTitle { get; set; }

        public string Description { get; set; }

        public string ModifiedAt { get; set; }
    }
}
