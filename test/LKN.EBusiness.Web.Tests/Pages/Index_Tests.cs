using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace LKN.EBusiness.Pages;

public class Index_Tests : EBusinessWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
