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
        public static Member ToMember(this Account user)
        {
            return new Member
            {
                Id = user.Id,
                NickName = user.UserName ?? "Unknown",
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

        public static T SortIfOrderable<T>(this T collection) where T : IList
        {
            if (collection == null || collection.Count == 0)
                return collection;

            var ordered = collection
                .OfType<IOrderable>()
                .OrderBy(x=> x.Order)
                .ToList();

            if (ordered.Count == 0)
                return collection;
            
            collection.Clear();
            foreach (var orderable in ordered)
                collection.Add(orderable);

            return collection;
        }
    }
}