using GraphQL.AspNetCore.Data;
using System.Linq;
using Xunit;

namespace GraphQl.AspNetCore.Data.Test
{
    public class UnitTest1
    {
        [Fact]
        public void GraphQLTypeBuilderTest()
        {
            var applicationDbContext = new MyTestDbContext();

            var graphTypeForMyClass = GraphQLTypeBuilder.CreateFor<MyClass>()
                  .FieldsFrom(applicationDbContext.Model)
                  // Additional configuration:
                  .Field(myClass => myClass.AnyProperty, with => with.Description("Description"))
                  .Field(myClass => myClass.Id, with => with.DefaultValue("Default"))
                  .Build();

            Assert.Equal(2, graphTypeForMyClass.Fields.ToArray().Length);
        }

        [Fact]
        public void GraphBuilderTest()
        {
            var applicationDbContext = new MyTestDbContext();

            var allGraphTypes = new GraphBuilder(applicationDbContext)
                .Define<MyClass>()
                .Define<OtherClass>(o => o
                    .Field(otherClass => otherClass.Test, with => with.Description("test"))
                    .Field(otherClass => otherClass.Id, with => with.Description("test").DefaultValue("7")))
                .BuildGraphTypes();

            Assert.Equal(4, allGraphTypes.Count);
        }
    }


    public class MyClass
    {

        public int Id { get; set; }

        public string AnyProperty { get; set; }

    }


    public class OtherClass
    {

        public int Id { get; set; }

        public string Test { get; set; }

        public string Abc { get; set; }

    }
}
