using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.Localization;
using Robust.Shared.Utility;

namespace Content.Server.Examine
{
    [UsedImplicitly]
    public class ExamineSystem : ExamineSystemShared
    {
        private static readonly FormattedMessage _entityNotFoundMessage;

        static ExamineSystem()
        {
            _entityNotFoundMessage = new FormattedMessage();
            _entityNotFoundMessage.AddText(Loc.GetString("examine-system-entity-does-not-exist"));
        }

        public override void Initialize()
        {
            base.Initialize();

            SubscribeNetworkEvent<ExamineSystemMessages.RequestExamineInfoMessage>(ExamineInfoRequest);
        }

        public override void SendExamineTooltip(EntityUid player, EntityUid target, FormattedMessage message, bool getVerbs, bool centerAtCursor)
        {
            if (!TryComp<ActorComponent>(player, out var actor))
                return;

            var session = actor.PlayerSession;

            var ev = new ExamineSystemMessages.ExamineInfoResponseMessage(
                target, message, getVerbs, centerAtCursor
            );

            RaiseNetworkEvent(ev, session.ConnectedClient);
        }

        private void ExamineInfoRequest(ExamineSystemMessages.RequestExamineInfoMessage request, EntitySessionEventArgs eventArgs)
        {
            var player = (IPlayerSession) eventArgs.SenderSession;
            var session = eventArgs.SenderSession;
            var channel = player.ConnectedClient;

            if (session.AttachedEntity is not {Valid: true} playerEnt
                || !EntityManager.EntityExists(request.EntityUid)
                || !CanExamine(playerEnt, request.EntityUid))
            {
                RaiseNetworkEvent(new ExamineSystemMessages.ExamineInfoResponseMessage(
                    request.EntityUid, _entityNotFoundMessage, request.GetVerbs), channel);
                return;
            }

            var text = GetExamineText(request.EntityUid, player.AttachedEntity);
            RaiseNetworkEvent(new ExamineSystemMessages.ExamineInfoResponseMessage(request.EntityUid, text, request.GetVerbs), channel);
        }
    }
}
