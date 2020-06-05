using Microsoft.Extensions.Logging;
using Moq;
using Plexure.Sample.Interfaces;
using Plexure.Sample.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Plexure.Sample.Tests
{

    /// <summary>
    /// Exercise 3:
    /// Write unit tests for the following class:
    /// </summary>
    public class CouponManagerTests
    {

        [Fact]
        public void Constructor_Should_ThrowException_When_CouponProvider_Is_Null()
        {
            //Arrange
            Mock<ILogger> logger = new Mock<ILogger>();

            //Assign
            var exception = Assert.Throws<ArgumentNullException>(() => new CouponManager(logger.Object, null));

            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'couponProvider')", exception.Message);
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_Logger_Is_Null()
        {
            //Arrange
            Mock<ICouponProvider> couponProvider = new Mock<ICouponProvider>();

            //Assign
            var exception = Assert.Throws<ArgumentNullException>(() => new CouponManager(null, couponProvider.Object));

            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'logger')", exception.Message);
        }

        [Fact]
        public async Task CanRedeemCoupon_Should_ThrowException_When_Evaluators_Is_Null()
        {
            //Arrange
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ICouponProvider> couponProvider = new Mock<ICouponProvider>();

            //Assign
            var couponManager = new CouponManager(logger.Object, couponProvider.Object);
            var couponGuid = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => couponManager.CanRedeemCoupon(couponGuid, userId, null));

            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'evaluators')", exception.Message);
        }

        [Fact]
        public async Task CanRedeemCoupon_Should_ThrowException_When_Coupon_Is_Null()
        {
            //Arrange
            Mock<ICouponProvider> couponProvider = new Mock<ICouponProvider>();
            Mock<IEnumerable<Func<Coupon, Guid, bool>>> evaluators = new Mock<IEnumerable<Func<Coupon, Guid, bool>>>();
            Mock<ILogger> logger = new Mock<ILogger>();

            couponProvider.Setup(x => x.Retrieve(It.IsAny<Guid>()))
                .Returns(Task.FromResult<Coupon>(null));

            //Assign
            var couponManager = new CouponManager(logger.Object, couponProvider.Object);
            var couponGuid = Guid.NewGuid();
            var userId = Guid.NewGuid();

            //Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => couponManager.CanRedeemCoupon(couponGuid, userId, evaluators.Object));
        }

        [Fact]
        public async Task CanRedeemCoupon_Should_Return_True_When_No_Evaluators()
        {
            //Arrange
            Mock<ICouponProvider> couponProvider = new Mock<ICouponProvider>();
            couponProvider.Setup(x => x.Retrieve(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Coupon()));

            IEnumerable<Func<Coupon, Guid, bool>> evaluators = new List<Func<Coupon, Guid, bool>>();
            Mock<ILogger> logger = new Mock<ILogger>();

            //Assign
            var couponManager = new CouponManager(logger.Object, couponProvider.Object);
            var couponGuid = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var canRedeemCoupon = await couponManager.CanRedeemCoupon(couponGuid, userId, evaluators);

            //Assert
            Assert.True(canRedeemCoupon);
        }

        [Fact]
        public async Task CanRedeemCoupon_Should_Return_True_For_Truthy_Evaluation()
        {
            //Arrange
            Mock<ICouponProvider> couponProvider = new Mock<ICouponProvider>();
            couponProvider.Setup(x => x.Retrieve(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Coupon()));

            List<Func<Coupon, Guid, bool>> evaluators = new List<Func<Coupon, Guid, bool>>();
            evaluators.Add((Coupon, Guid) => { return true; });

            Mock<ILogger> logger = new Mock<ILogger>();

            //Assign
            var couponManager = new CouponManager(logger.Object, couponProvider.Object);
            var couponGuid = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var canRedeemCoupon = await couponManager.CanRedeemCoupon(couponGuid, userId, evaluators);

            //Assert
            Assert.True(canRedeemCoupon);
        }

        [Fact]
        public async Task CanRedeemCoupon_Should_Return_False_For_Evaluators_Containing_Falsey_Evaluation()
        {
            //Arrange
            Mock<ICouponProvider> couponProvider = new Mock<ICouponProvider>();
            couponProvider.Setup(x => x.Retrieve(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Coupon()));

            List<Func<Coupon, Guid, bool>> evaluators = new List<Func<Coupon, Guid, bool>>();
            evaluators.Add((Coupon, Guid) => { return true; });
            evaluators.Add((Coupon, Guid) => { return false; });

            Mock<ILogger> logger = new Mock<ILogger>();

            //Assign
            var couponManager = new CouponManager(logger.Object, couponProvider.Object);
            var couponGuid = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var canRedeemCoupon = await couponManager.CanRedeemCoupon(couponGuid, userId, evaluators);

            //Assert
            Assert.True(canRedeemCoupon);
        }

        [Fact]
        public async Task CanRedeemCoupon_Should_Return_False_For_Falsey_Evaluation()
        {
            //Arrange
            Mock<ICouponProvider> couponProvider = new Mock<ICouponProvider>();
            couponProvider.Setup(x => x.Retrieve(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Coupon()));

            List<Func<Coupon, Guid, bool>> evaluators = new List<Func<Coupon, Guid, bool>>();
            evaluators.Add((Coupon, Guid) => { return false; });

            Mock<ILogger> logger = new Mock<ILogger>();

            //Assign
            var couponManager = new CouponManager(logger.Object, couponProvider.Object);
            var couponGuid = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var canRedeemCoupon = await couponManager.CanRedeemCoupon(couponGuid, userId, evaluators);

            //Assert
            Assert.False(canRedeemCoupon);
        }

    }
}