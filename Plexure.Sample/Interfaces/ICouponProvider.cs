using Plexure.Sample.Models;
using System;
using System.Threading.Tasks;

namespace Plexure.Sample.Interfaces
{
    public interface ICouponProvider
    {
        /// <summary>
        /// Retrieves a coupon using the couponId
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        Task<Coupon> Retrieve(Guid couponId);
    }
}