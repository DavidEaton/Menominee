using System;

namespace Menominee.Shared.Models.Http
{
    public class PostResponse
    {
        public long Id { get; set; }

        public static implicit operator long(PostResponse postResponse)
        {
            if (postResponse is null)
            {
                throw new ArgumentNullException(nameof(postResponse), "Cannot convert null PostResponse to long.");
            }

            return postResponse.Id;
        }
    }
}
