// Copyright (c) Adam Nathan.  All rights reserved.
//
// By purchasing the book that this source code belongs to, you may use and
// modify this code for commercial and non-commercial applications, but you
// may not publish the source code.
// THE SOURCE CODE IS PROVIDED "AS IS", WITH NO WARRANTIES OR INDEMNITIES.
using System;
using System.Windows.Media;
using System.Windows.Resources;
using Microsoft.Xna.Framework.Audio; // For SoundEffect
using System.Windows;

namespace WindowsPhoneApp
{
  public static class SoundEffects  {
    public static void Initialize()
    {
      StreamResourceInfo info;

      info = Application.GetResourceStream(new Uri("Audio/punch1.wav", UriKind.Relative));
      Punch1 = SoundEffect.FromStream(info.Stream);

      info = Application.GetResourceStream(new Uri("Audio/punch2.wav", UriKind.Relative));
      Punch2 = SoundEffect.FromStream(info.Stream);

      info = Application.GetResourceStream(new Uri("Audio/punch3.wav", UriKind.Relative));
      Punch3 = SoundEffect.FromStream(info.Stream);

      info = Application.GetResourceStream(new Uri("Audio/punch4.wav", UriKind.Relative));
      Punch4 = SoundEffect.FromStream(info.Stream);

      info = Application.GetResourceStream(new Uri("Audio/grunt1.wav", UriKind.Relative));
      Grunt1 = SoundEffect.FromStream(info.Stream);

      info = Application.GetResourceStream(new Uri("Audio/grunt2.wav", UriKind.Relative));
      Grunt2 = SoundEffect.FromStream(info.Stream);

      info = Application.GetResourceStream(new Uri("Audio/grunt3.wav", UriKind.Relative));
      Grunt3 = SoundEffect.FromStream(info.Stream);

      info = Application.GetResourceStream(new Uri("Audio/dingDingDing.wav",
                                           UriKind.Relative));
      DingDingDing = SoundEffect.FromStream(info.Stream);

      CompositionTarget.Rendering += delegate(object sender, EventArgs e)
      {
        // Required for XNA Sound Effect API to work
        Microsoft.Xna.Framework.FrameworkDispatcher.Update();
      };

      // Call also once at the beginning
      Microsoft.Xna.Framework.FrameworkDispatcher.Update();
    }

    public static SoundEffect Punch1 { get; private set; }
    public static SoundEffect Punch2 { get; private set; }
    public static SoundEffect Punch3 { get; private set; }
    public static SoundEffect Punch4 { get; private set; }
    public static SoundEffect Grunt1 { get; private set; }
    public static SoundEffect Grunt2 { get; private set; }
    public static SoundEffect Grunt3 { get; private set; }
    public static SoundEffect DingDingDing { get; private set; }
  }
}
