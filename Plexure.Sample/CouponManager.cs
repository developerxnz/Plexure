using Microsoft.Extensions.Logging;
using Plexure.Sample.Interfaces;
using Plexure.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plexure.Sample
{

    public class CouponManager
    {

        private readonly ILogger _logger;

        private readonly ICouponProvider _couponProvider;

        public CouponManager(ILogger logger, ICouponProvider couponProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _couponProvider = couponProvider ?? throw new ArgumentNullException(nameof(couponProvider));
        }

        public async Task<bool> CanRedeemCoupon(Guid couponId, Guid userId, IEnumerable<Func<Coupon, Guid, bool>> evaluators)
        {
            if (evaluators == null)

                throw new ArgumentNullException(nameof(evaluators));

            var coupon = await _couponProvider.Retrieve(couponId);

            if (coupon == null)

                throw new KeyNotFoundException();

            if (!evaluators.Any())

                return true;

            bool result = false;

            foreach (var evaluator in evaluators)

                result |= evaluator(coupon, userId);

            return result;

        }
    }

}