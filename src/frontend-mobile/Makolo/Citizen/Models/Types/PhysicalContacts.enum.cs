using Citizen.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Types
{
    public enum PhysicalContacts
    {
        [EnumLangualKey("EnumPhysicalContact_Daughter")]
        Family_Daughter = 0,
        [EnumLangualKey("EnumPhysicalContact_Family_Son")]
        Family_Son,
        [EnumLangualKey("EnumPhysicalContact_Family_Wife")]
        Family_Wife,
        [EnumLangualKey("EnumPhysicalContact_Family_Husband")]
        Family_Husband,
        [EnumLangualKey("EnumPhysicalContact_Family_Mother")]
        Family_Mother,
        [EnumLangualKey("EnumPhysicalContact_Family_Father")]
        Family_Father,
        [EnumLangualKey("EnumPhysicalContact_Family_GrandMother")]
        Family_GrandMother,
        [EnumLangualKey("EnumPhysicalContact_Family_GrandFather")]
        Family_GrandFather,
        [EnumLangualKey("EnumPhysicalContact_Family_Nephew")]
        Family_Nephew,
        [EnumLangualKey("EnumPhysicalContact_Family_Niece")]
        Family_Niece,
        [EnumLangualKey("EnumPhysicalContact_Family_Aunt")]
        Family_Aunt,
        [EnumLangualKey("EnumPhysicalContact_Family_Uncle")]
        Family_Uncle,
        [EnumLangualKey("EnumPhysicalContact_Family_worker")]
        Family_worker,
        [EnumLangualKey("EnumPhysicalContact_Friend_Female")]
        Friend_Female,
        [EnumLangualKey("EnumPhysicalContact_Friend_Male")]
        Friend_Male,
        [EnumLangualKey("EnumPhysicalContact_Neighbor_Male")]
        Neighbor_Male,
        [EnumLangualKey("EnumPhysicalContact_Neighbor_Female")]
        Neighbor_Female,
        [EnumLangualKey("EnumPhysicalContact_City_Male")]
        City_Male,
        [EnumLangualKey("EnumPhysicalContact_City_Female")]
        City_Female
    }
}
