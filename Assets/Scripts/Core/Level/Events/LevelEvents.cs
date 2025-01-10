using UnityEngine;
using Core.Base.Event;
using Core.Level.Data;
using Core.Level.Interface;

namespace Core.Level.Events
{
    public class LevelUpEvent
    {
        public ILevelable Source { get; private set; }
        public int NewLevel { get; private set; }

        public LevelUpEvent(ILevelable source, int newLevel)
        {
            Source = source;
            NewLevel = newLevel;
        }
    }

    public class PrestigeEvent
    {
        public ILevelable Source { get; private set; }
        public int PrestigeLevel { get; private set; }

        public PrestigeEvent(ILevelable source, int prestigeLevel)
        {
            Source = source;
            PrestigeLevel = prestigeLevel;
        }
    }

    public class ExperienceGainEvent
    {
        public ILevelable Source { get; private set; }
        public float Amount { get; private set; }

        public ExperienceGainEvent(ILevelable source, float amount)
        {
            Source = source;
            Amount = amount;
        }
    }

    public class ContentUnlockEvent
    {
        public ILevelable Source { get; private set; }
        public LevelData.UnlockableContent[] Content { get; private set; }

        public ContentUnlockEvent(ILevelable source, LevelData.UnlockableContent[] content)
        {
            Source = source;
            Content = content;
        }
    }
} 