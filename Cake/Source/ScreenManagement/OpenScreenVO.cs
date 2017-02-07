
using System;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// VO with the view identification and maybe the information for it to display
    /// </summary>
    public class OpenScreenVO
    {
        /// <summary>
        /// Evil static number of instances.
        /// </summary>
        private static int instanceCount;

        public ScreenIdentifier Id{ get; private set; }
        private readonly int instanceIdentifier;

        /// <summary>
        /// Any VO with information for the View, preferably some Ids so that the Mediator can fetch the updated information from the models
        /// </summary>
        public IScreenPropertiesVO Properties { get; protected set; }

        /// <summary>
        /// Marks if a open or close animation should be skipped
        /// </summary>
        public bool PreventAnimation { get; set; }

        /// <summary>
        /// This tell the layer to put the gameobject as the second child
        /// (first or last)
        /// </summary>
        public bool SetAsSecondSibling { get; set; }

        public OpenScreenVO(ScreenIdentifier id, IScreenPropertiesVO properties = null)
        {
            Properties = properties;
            Id = id;

            instanceCount++;
            instanceIdentifier = instanceCount;
        }


        protected void ReplaceContents(OpenScreenVO newVO)
        {
            if (newVO.Id != Id)
            {
                throw new ArgumentException("Replacing content for different panel Id is wrong, you should be arrested!");
            }
            Properties = newVO.Properties;
        }

        public int InstanceIdentifier
        {
            get
            {
                return instanceIdentifier;
            }
        }
    }
}