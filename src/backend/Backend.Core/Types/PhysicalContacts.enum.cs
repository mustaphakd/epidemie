using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Types
{
    /// <summary>
    /// Physical contact type
    /// </summary>
    public enum PhysicalContacts
    {
        /// <summary>
        /// Daughter
        /// </summary>
        Family_Daughter = 0,
        /// <summary>
        /// Son
        /// </summary>
        Family_Son,
        /// <summary>
        /// WIfe
        /// </summary>
        Family_Wife,
        /// <summary>
        /// Husband
        /// </summary>
        Family_Husband,
        /// <summary>
        /// Mother
        /// </summary>
        Family_Mother,
        /// <summary>
        /// Father
        /// </summary>
        Family_Father,
        /// <summary>
        /// Grand mother
        /// </summary>
        Family_GrandMother,
        /// <summary>
        /// Grand father
        /// </summary>
        Family_GrandFather,
        /// <summary>
        /// Nephew
        /// </summary>
        Family_Nephew,
        /// <summary>
        /// Niece
        /// </summary>
        Family_Niece,
        /// <summary>
        /// Aunt
        /// </summary>
        Family_Aunt,
        /// <summary>
        /// Uncle
        /// </summary>
        Family_Uncle,
        /// <summary>
        /// Co-worker
        /// </summary>
        Family_worker,
        /// <summary>
        /// Female friend
        /// </summary>
        Friend_Female,
        /// <summary>
        /// Male friend
        /// </summary>
        Friend_Male,
        /// <summary>
        /// Male from around the community
        /// </summary>
        Neighbor_Male,
        /// <summary>
        /// Female from around the community
        /// </summary>
        Neighbor_Female,
        /// <summary>
        /// Unkown male from current city
        /// </summary>
        City_Male,
        /// <summary>
        /// Unkown female from current city
        /// </summary>
        City_Female
    }
}
