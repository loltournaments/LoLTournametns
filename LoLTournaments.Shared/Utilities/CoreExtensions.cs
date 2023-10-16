using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Shared.Utilities
{
    public static class CoreExtensions
    {
        public static Member ToMember(this UserDto user)
        {
            return new Member
            {
                Id = user.Id,
                NickName = user.Name ?? "Unknown",
                State = user.Permission.ToMemberState()
            };
        }
        
        public static Winner ToWinner(this Member member, int place)
        {
            return new Winner
            {
                Id = member.Id,
                NickName = member.NickName ?? "Unknown",
                State = member.State,
                Order = place
            };
        }

        public static MemberState ToMemberState(this Permissions permissions)
        {
            return permissions.HasAnyFlags(Permissions.Participant | Permissions.Developer | Permissions.Manager)
                ? MemberState.Admitted 
                : MemberState.NotEligible;
        }

        public static bool IsAllFinished(this IEnumerable<Game> games)
        {
            return games == null || games.All(x => x.IsFinished());
        }
        
        public static MemberStat GetMemberStat(this IEnumerable<Game> games, string memberId)
        {
            var totalGames = (games?.Where(x => x.Members.Contains(memberId))).ToListFix();
            return new MemberStat
            {
                Win = totalGames.Count(x => x.IsFinished() && x.WinnerId == memberId),
                Lose = totalGames.Count(x => x.IsFinished() && x.WinnerId != memberId)
            };
        }

        public static bool IsFinished(this Game game)
        {
            return game == null || game.WinnerReason != WinnerReason.None;
        }

        public static void Move<T>(this List<T> source, string id, int index = 0) where T : IIdentity
        {
            if (string.IsNullOrEmpty(id) || source.IsNullOrEmpty())
                return;

            var identity = source.FirstOrDefault(x => x.Id == id);
            source.Move(identity, index);
        }

        public static void Move<T>(this List<T> source, T item, int index = 0)
        {
            if (item == null || source == null)
                return;
            
            index = Math.Clamp(index, 0, source.Count - 1);
            if (source.Remove(item))
                source.Insert(index, item);
        }
        
        public static bool Replace<T>(this IList<T> source, T item) where T : IIdentity
        {
            if (source == null)
            {
                DefaultSharedLogger.Error($"Source List<{typeof(T).Name}> does not exist");
                return false;
            }
            
            if (item == null)
            {
                DefaultSharedLogger.Error("Item does not exist");
                return false;
            }
            
            var index = source.IndexOf(x => x.Id == item.Id);
            index = index <= 0 ? Math.Max(0,source.Count-1) : index;
            foreach (var identity in source.ToArray())
            {
                if (identity?.Id != item.Id)
                    continue;

                source.Remove(identity);
            }

            source.Insert(index, item);
            return true;
        }
        
        public static bool Replace<T>(this IList<T> source, IEnumerable<T> items) where T : IIdentity
        {
            return items != null && source != null && items.All(source.Replace);
        }
        
        public static OrderedDictionary ToOrderedDictionary(this IEnumerable enumerable)
        {
            if (enumerable == null)
                return new OrderedDictionary();
            
            var dictionary = new OrderedDictionary();
            foreach (var item in enumerable)
            {
                if (item is not IIdentity identity)
                    identity = default;

                var key = identity?.Id ?? item.ToString();
                if(dictionary.Contains(key))
                    continue;
                
                dictionary.Add(key, item);
            }

            return dictionary;
        }

        public static OrderedDictionary ToOrderedDictionary(this IEnumerable<IOrderable> orderables)
        {
            if (orderables == null)
                return new OrderedDictionary();
            
            var enumarable = (IEnumerable)orderables.OrderBy(x => x.Order);
            return enumarable.ToOrderedDictionary();
        }

        public static void SortIfOrderable(this IList collection)
        {
            if (collection == null)
                return;

            var listType = collection.GetType();
            if (listType.GenericTypeArguments.IsNullOrEmpty())
                return;
            
            var genericType = collection.GetType().GetGenericArguments()[0];
            if (!typeof(IOrderable).IsAssignableFrom(genericType)) 
                return;
                
            var ordered = collection
                .Cast<IOrderable>()
                .OrderBy(x=> x.Order)
                .ToListFix();
                
            collection.Clear();
            foreach (var orderable in ordered)
                collection.Add(orderable);
        }
    }
}