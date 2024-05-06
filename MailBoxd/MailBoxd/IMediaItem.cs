using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBoxd
{
    public interface IMediaItem
    {
        string Titre { get; }
        string Genre { get; }
        int Annee { get; }
        bool IsInWishlist { get; }
    }
}
