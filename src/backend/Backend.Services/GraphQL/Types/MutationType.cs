using Backend.DataTransferObjects;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Services.GraphQL.Types
{
    public class MutationType
        : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(t => t.AddPhysicalContacts(default, default))
                .Type<ListType<SynchronizationTokenType>>()
                .Argument("contacts", a => a.Type<NonNullType<ListType<PhysicalContactType>>>());
                //.Argument("review", a => a.Type<NonNullType<ReviewInputType>>());
        }
    }
}
