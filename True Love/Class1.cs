using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace True_Love
{
    class Class1
    {
        TileContent content = new TileContent()
        {
            Visual = new TileVisual()
            {

                TileWide = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        TextStacking = TileTextStacking.Center,
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Jennifer Parker",
                                HintStyle = AdaptiveTextStyle.Subtitle,
                                HintWrap = true
                             },

                            new AdaptiveText() {
                                Text = "Photos from our trip",
                                HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                HintWrap = true
                             },

                            new AdaptiveText()
                            {
                                 Text = "Check out these awesome photos I took while in New Zealand!",
                                 HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                 HintWrap = true
                            }
                        }
                    }
                },
            }
        };

    }
}
