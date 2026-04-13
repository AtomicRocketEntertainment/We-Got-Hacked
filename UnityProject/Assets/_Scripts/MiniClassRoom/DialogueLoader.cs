using MiniClassRoom;
using System.Collections.Generic;
using UnityEngine;

namespace MiniClassRoom
{
    public class DialogueLoader : MonoBehaviour
    {
        [SerializeField] private TextAsset _csvFile;
        [SerializeField] private List<ActorSO> _actorsDatabase;

        public List<DialogueLine> LoadDialogue()
        {
            List<DialogueLine> lines = new();

            string[] rows = _csvFile.text.Split('\n');

            for (int i = 0; i < rows.Length; i++)
            {
                string[] cols = rows[i].Split(',');

                DialogueLine line = new DialogueLine();
                line.actors = new List<ActorData>();

                // Primeiro ator
                if (!string.IsNullOrEmpty(cols[1]))
                {
                    line.actors.Add(CreateActor("Rq", cols[1], cols[2]));
                }

                // Segundo ator
                if (!string.IsNullOrEmpty(cols[3]))
                {
                    line.actors.Add(CreateActor("Rb", cols[3], cols[4]));
                }

                if (!string.IsNullOrEmpty(cols[5]))
                {
                    line.actors.Add(CreateActor("Rf", cols[5], cols[6]));
                }

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