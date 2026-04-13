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

            for (int i = 2; i < rows.Length; i++) // pula header
            {
                string[] cols = rows[i].Split(';');

                DialogueLine line = new DialogueLine();
                line.actors = new List<ActorData>();

                // Primeiro ator
                if (!string.IsNullOrEmpty(cols[1]))
                {
                    line.actors.Add(CreateActor(cols[1], cols[2], cols[3]));
                }

                // Segundo ator
                if (!string.IsNullOrEmpty(cols[4]))
                {
                    line.actors.Add(CreateActor(cols[4], cols[5], cols[6]));
                }

                line.actorHighlightedID = cols[7];
                line.dialogueText = cols[9];

                lines.Add(line);
            }

            return lines;
        }

        private ActorData CreateActor(string actorID, string headID, string bodyID)
        {
            ActorSO actor = _actorsDatabase.Find(a => a.id == actorID);

            return new ActorData
            {
                actor = actor,
                headID = headID,
                bodyID = bodyID
            };
        }
    }
}