using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockBlaster.Performance
{
    public interface IEntityFactory
    {
        object CreateNew();
        object CreateNew(FlatRedBall.Graphics.Layer layer);

        void Initialize(string contentManager);
        void ClearListsToAddTo();
    }
}
