// This file is a part of MPDN Extensions.
// https://github.com/zachsaw/MPDN_Extensions
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library.
// 
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mpdn.Extensions.Framework;

namespace Mpdn.Extensions.PlayerExtensions
{
    public class GoToTime : PlayerExtension
    {
        private readonly PlayerMenuItem m_MenuItem = new PlayerMenuItem(initiallyDisabled: true);

        public override ExtensionUiDescriptor Descriptor
        {
            get
            {
                return new ExtensionUiDescriptor
                {
                    Guid = new Guid("7C3BA1E2-EE7B-47D2-B174-6AE76D65EC04"),
                    Name = "Go To Time",
                    Description = "Jump to a specified timecode in media"
                };
            }
        }

        public override IList<Verb> Verbs
        {
            get
            {
                return new[]
                {
                    new Verb(Category.Play, string.Empty, "Go To...", "Ctrl+G", string.Empty, GotoPosition, m_MenuItem)
                };
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            PlayerControl.PlayerStateChanged += PlayerStateChanged;
        }

        public override void Destroy()
        {
            PlayerControl.PlayerStateChanged -= PlayerStateChanged;
            base.Destroy();
        }

        private void PlayerStateChanged(object sender, PlayerStateEventArgs e)
        {
            m_MenuItem.Enabled = e.NewState != PlayerState.Closed;
        }

        private void GotoPosition()
        {
            if (!m_MenuItem.Enabled)
                return;

            using (var form = new GoToTimeForm())
            {
                if (form.ShowDialog(PlayerControl.VideoPanel) != DialogResult.OK)
                    return;

                if (PlayerControl.PlayerState == PlayerState.Closed)
                    return;

                if (PlayerControl.PlayerState == PlayerState.Stopped)
                {
                    PlayerControl.PauseMedia(false);
                }

                PlayerControl.SeekMedia(form.Position * 1000);
            }
        }
    }
}
