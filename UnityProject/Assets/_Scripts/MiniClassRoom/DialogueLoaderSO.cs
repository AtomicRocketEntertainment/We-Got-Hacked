using System;
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
        public List<Sprite> _backgrounds;

        public List<DialogueLine> LoadDialogue()
        {
            List<DialogueLine> lines = new List<DialogueLine>();

            string[] rows = _csvFile.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < rows.Length; i++)
            {
                if (rows == null || rows.Count() == 0) break;

                string actorId = "";
                string bodyId = "";
                string headId = "";
                Sprite background = null;

                string[] cols = rows[i].Split(';');

                DialogueLine line = new DialogueLine();
                line.actors = new List<ActorData>();

                //Seta background
                if(_backgrounds.Count > 0)
                {
                    if (int.TryParse(cols[0], out int bgIndex))
                    {
                        background = (bgIndex >= 0 && bgIndex < _backgrounds.Count) ? _backgrounds[bgIndex] : _backgrounds[0];
                    }
                }
                line.background = background;

                // Primeiro ator (Raquel)
                //if (!string.IsNullOrEmpty(cols[1]))
                actorId = _actorsDatabase[0].id;
                bodyId = (cols[1] != null) ? cols[1] : "idle";
                headId = (cols[2] != null) ? cols[2] : "idle";

                line.actors.Add(CreateActor(actorId, bodyId, headId));

                // Segundo ator (Rebeca)
                //if (!string.IsNullOrEmpty(cols[3]))
                actorId = _actorsDatabase[1].id;
                bodyId = (cols[3] != null) ? cols[3] : "idle";
                headId = (cols[4] != null) ? cols[4] : "idle";
                line.actors.Add(CreateActor(actorId, bodyId, headId));

                // Terceiro ator (Rafael)
                //if (!string.IsNullOrEmpty(cols[5]))
                actorId = _actorsDatabase[2].id;
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
