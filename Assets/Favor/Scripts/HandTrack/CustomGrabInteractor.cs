using Oculus.Interaction.HandGrab;
using System;


public class CustomGrabInteractor : HandGrabInteractor
{
    public override bool IsGrabbing
    {
        get
        {
            if (base.IsGrabbing)
            {
                CustomOnGrab.Invoke();
            }
            else
            {
                CustomOnRelease.Invoke();
            }
            return base.IsGrabbing;
        }
    }

    public event Action CustomOnGrab;
    public event Action CustomOnRelease;

}
