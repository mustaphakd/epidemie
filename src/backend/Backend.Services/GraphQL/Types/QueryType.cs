using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Services.GraphQL.Types
{
    public class QueryType
        : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            /*
            descriptor.Field(t => t.GetHero(default))
                .Type<CharacterType>()
                .Argument("episode", a => a.DefaultValue(Episode.NewHope));

            descriptor.Field(t => t.GetCharacter(default, default))
                .Type<NonNullType<ListType<NonNullType<CharacterType>>>>();

            // the search can only be executed if the current
            // identity has a country
            descriptor.Field(t => t.Search(default))
                .Type<ListType<SearchResultType>>()
                .Authorize("HasCountry");*/

            descriptor.Name("Makolo");

            descriptor.Field(t => t.GetHello())
                .Name("greetings");
        }
    }
}
