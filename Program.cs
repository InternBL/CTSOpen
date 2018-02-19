using System;
using System.Linq;
using CTSOpen.NX.Features;
using NXOpen;
using Snap;

namespace CTSOpen
{
    public class Program
    {
        public static void Main()
        {
            try
            {

                //UFSession.GetUFSession().Curve.

                foreach (var sheet in CTSOpen.Globals.DisplayPart.Features.OfType<LinkedBody>())
                {
                    //InfoWindow.WriteLine(sheet.SourcePart);
                    //InfoWindow.WriteLine(sheet.Name);
                }

                //var component = new Component(Snap.NX.NXObject.FindByName("110").NXOpenTag);
                //foreach (var member in component.Members)
                //{
                //    var obj = member as CTSOpen.DisplayableObject;
                //    if (obj == null) continue;
                //    obj.Highlighted = true;
                //}
            }
            catch (Exception ex)
            {
                InfoWindow.WriteLine(ex.Message);
            }
        }

        public static int GetUnloadOption(string arg) { return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately); }
    }
}