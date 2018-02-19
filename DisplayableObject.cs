using System;
using NXOpen;
using NXOpen.UF;

namespace CTSOpen.NX
{
    public abstract class DisplayableObject : NXObject
    {
        protected DisplayableObject(Tag tag) : base(tag) { }

        private UFObj.DispProps DispProps
        {
            get
            {
                UFObj.DispProps dispProps;
                UFSession.GetUFSession().Obj.AskDisplayProperties(Tag, out dispProps);
                return dispProps;
            }
        }

        public int Layer
        {
            get { return DispProps.layer; }
            set
            {

                if (UFConstants.UF_LAYER_MIN_LAYER <= value && value <= UFConstants.UF_LAYER_MAX_LAYER)
                    throw new ArgumentOutOfRangeException("value",
                        "Number " + value + " is not a valid Layer. Inclusive " + UFConstants.UF_LAYER_MIN_LAYER + " - " + UFConstants.UF_LAYER_MAX_LAYER +
                        ".");
                UFSession.GetUFSession().Obj.SetLayer(Tag, value);
            }
        }

        public bool Blanked
        {
            get
            {
                switch (DispProps.blank_status)
                {
                    case UFConstants.UF_OBJ_BLANKED:
                        return true;
                    case UFConstants.UF_OBJ_NOT_BLANKED:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (value)
                {
                    case true:
                        UFSession.GetUFSession().Obj.SetBlankStatus(Tag, UFConstants.UF_OBJ_BLANKED);
                        break;
                    case false:
                        UFSession.GetUFSession().Obj.SetBlankStatus(Tag, UFConstants.UF_OBJ_NOT_BLANKED);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int Color { get { return DispProps.color; } }

        public bool Highlighted
        {
            get { return DispProps.highlight_status; }
            set
            {
                // todo: UFSession
                switch (value)
                {
                    case true:
                        ((NXOpen.DisplayableObject) NXOpen.Utilities.NXObjectManager.Get(this)).Highlight();
                        break;
                    case false:
                        ((NXOpen.DisplayableObject) NXOpen.Utilities.NXObjectManager.Get(this)).Unhighlight();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}