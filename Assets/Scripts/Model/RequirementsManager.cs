using System.Collections.Generic;
using System.Linq;
using Match3;
using Match3.Model;
using UnityEngine;

namespace Model
{
    public class RequirementsManager
    {
        /// <summary>
        /// Requirements are used for the level
        /// </summary>
        public static readonly List<Requirement> Requirements = new List<Requirement>();
        
        
        public static void ClearRequirements()
        {
            if (Requirements.Count < 1) {
                return;
            }
            foreach (var t in Requirements) {
                t.transform.gameObject.SetActive(false);
            }
            ResetRequirements();
        }

        public static void ResetRequirements()
        {
            if (Requirements.Count < 1) {
                return;
            }
            foreach (var t in Requirements) {
                t.ResetCurrentProgress();
            }

        }

        /// <summary>
        /// Fetching Position of the Requirement to animate particle element 
        /// </summary>
        public static Transform GetPositionForRequirement(int type)
        {
            var selectedRequirement =  Requirements.Where(i => i.pieceColorNumber == type);
            return selectedRequirement.Select(iRequirement => iRequirement.transform).FirstOrDefault();
        }
    }
}