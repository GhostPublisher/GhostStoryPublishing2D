using System;
using System.Collections.Generic;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UnitSystem.GUIDSystem
{
    public class GUIDAllocator : IUtilityReferenceHandler
    {
        private static HashSet<int> existingIDs = new();

        public int GenerateUniqueIntID()
        {
            int newID;

            do
            {
                newID = CreateStableHash(Guid.NewGuid());
            } while (existingIDs.Contains(newID));

            existingIDs.Add(newID);
            return newID;
        }

        private int CreateStableHash(Guid guid)
        {
            byte[] bytes = guid.ToByteArray();
            int hash = 17;
            foreach (byte b in bytes)
            {
                hash = hash * 31 + b;
            }
            return hash;
        }

        /// ���� ID ���
        public void RegisterExistingID(int id)
        {
            existingIDs.Add(id);
        }

        /// ���� ID ���
        public void RegisterExistingIDs(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                existingIDs.Add(id);
            }
        }

        /// ���� ID ����
        public void UnregisterID(int id)
        {
            existingIDs.Remove(id);
        }

        /// ���� ID ����
        public void UnregisterIDs(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                existingIDs.Remove(id);
            }
        }

        /// ��ü �ʱ�ȭ (������)
        public void ClearAllIDs()
        {
            existingIDs.Clear();
        }
    }

}