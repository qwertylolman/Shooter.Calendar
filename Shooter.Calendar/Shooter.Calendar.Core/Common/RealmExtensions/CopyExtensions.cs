﻿using System;
using Realms;
using MvvmCross;

namespace Shooter.Calendar.Core.Common.RealmExtensions
{
    public static class CopyExtensions
    {
        private static ICopyManager copyManager;

        public static ICopyManager CopyManager
            => copyManager ?? (copyManager = Mvx.IoCProvider.Resolve<ICopyManager>());

        public static TItem CopyItem<TItem>(this TItem realmObject)
            where TItem : RealmObject
            => CopyItem(realmObject as RealmObject) as TItem;

        public static RealmObject CopyItem(this RealmObject realmObject)
            => realmObject == null ? null : CopyManager.MakeACopy(realmObject);
    }
}
