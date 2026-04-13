using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniClassRoom
{
    [CreateAssetMenu(fileName = "Meeting Person", menuName = "Scriptable Objcts/MiniClass/DialogueLoader")]
    public class DialogueLoaderSO : ScriptableObject
    {
        public TextAsset _csvFile;
        public List<ActorSO> _actorsDatabase;

        public List<DialogueLine> LoadDialogue()
        {
            List<DialogueLine> lines = new List<DialogueLine>();

            string[] rows = _csvFile.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < rows.Length; i++)
            {

                if (rows == null || rows.Count() == 0) break;

                string actorId = "";
                string bodyId = "";
                string headId = "";

                string[] cols = rows[i].Split(',');

                DialogueLine line = new DialogueLine();
                line.actors = new List<ActorData>();

                // Primeiro ator (Raquel)
                //if (!string.IsNullOrEmpty(cols[1]))
                actorId = "Rq";
                bodyId = (cols[1] != null) ? cols[1] : "idle";
                headId = (cols[2] != null) ? cols[2] : "idle";

                line.actors.Add(CreateActor(actorId, bodyId, headId));

                // Segundo ator (Rebeca)
                //if (!string.IsNullOrEmpty(cols[3]))
                actorId = "Rb";
                bodyId = (cols[3] != null) ? cols[3] : "idle";
                headId = (cols[4] != null) ? cols[4] : "idle";
                line.actors.Add(CreateActor(actorId, bodyId, headId));

                // Terceiro ator (Rafael)
                //if (!string.IsNullOrEmpty(cols[5]))
                actorId = "Rf";
                bodyId = (cols[5] != null) ? cols[5] : "idle";
                headId = (cols[6] != null) ? cols[6] : "idle";
                line.actors.Add(CreateActor(actorId, bodyId, headId));

                line.actorHighlightedID = cols[7];
                line.dialogueText = cols[8];

                lines.Add(line);
            }

            return lines;
        }

        private ActorData CreateActor(string actorID, string bodyID, string headID)
        {
            ActorSO actor = _actorsDatabase.Find(a => a.id == actorID);

            if (actor == null) return null;

            return new ActorData
            {
                actor = actor,
                headID = headID,
                bodyID = bodyID
            };
        }
    }

}
