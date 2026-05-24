using System.Linq;
using VLimat.Eduz.App.Controllers.Master;
using Xunit;

namespace VLimat.Eduz.App.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Get_ReturnsExpectedNames()
        {
            var controller = new HomeController();

            var result = controller.Get();

            Assert.NotNull(result);
            var array = result.ToArray();
            Assert.Equal(2, array.Length);
            Assert.Equal("Rahul", array[0]);
            Assert.Equal("Ayyappan", array[1]);
        }

        [Fact]
        public void Get_ById_ReturnsValue()
        {
            var controller = new HomeController();

            var result = controller.Get(5);

            Assert.Equal("value", result);
        }

        [Fact]
        public void Post_Put_Delete_DoNotThrow()
        {
            var controller = new HomeController();

            var ex = Record.Exception(() =>
            {
                controller.Post("test");
                controller.Put(1, "test");
                controller.Delete(1);
            });

            Assert.Null(ex);
        }
    }
}