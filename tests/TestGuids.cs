﻿using System;
using System.Linq;

namespace FastHash.Tests
{
    public static class TestGuids
    {
        public static Guid[] Get()
        {
            return GetGuidStrings().Select(Guid.Parse).ToArray();
        }

        private static string[] GetGuidStrings()
        {
            return new[]
            {
                "63c29095-e593-4dbd-8df2-821dee9430ae",
                "473f49a3-a6e0-4eb8-9de9-8148c7a04f2e",
                "bf1184a4-bb82-4e5a-9e19-0e93d3f28500",
                "fd855095-ba8c-4c0c-bea4-fd48d6d0bdc5",
                "52ac3813-bdac-47aa-8717-63b7f463a235",
                "0d6e2ac8-b6a2-4a59-8d96-51e3b21418cb",
                "2e64bb44-140d-4ae7-81d5-13c596ca4e72",
                "baa60612-2428-4067-8266-b6bdb2621d94",
                "cfe97e1d-aae6-492a-ad19-b2812b76b770",
                "eae8b5a3-301a-40b9-925e-6782f2394244",
                "d2dd9e96-72e8-4462-8f84-e8543c9c5670",
                "fc290043-68db-4975-96d8-7891b5ba4276",
                "3044b306-652b-41de-a8e6-e65b893dfb63",
                "38f3ff56-d31d-41a1-804a-22c440e3d65e",
                "a15c3df4-7395-4ba5-b99a-633aadfa7ed7",
                "c3f7e6b9-08f4-4610-8fc8-15b1f74e0587",
                "27f4f043-35c9-4ad6-99b0-be8850a8be10",
                "4cae5853-752a-4483-958d-5028d80cf5e7",
                "49ed62fa-53a3-469d-a444-6edb6af0b9cb",
                "d6aa98c8-84be-4995-a525-ba802726f1ec",
                "aee78346-a8fe-4d52-b118-850b0fb7c574",
                "6f557f8a-6912-4bfc-9373-f47deccf3f23",
                "e3efee30-94be-4cd6-bdb3-d0b5e3b3a917",
                "030ed62b-17ae-4759-baa5-4cd9d5c30b4c",
                "a7a9190e-96b1-43c9-bc17-086e07887a1e",
                "2af52ef7-0144-48b0-bd6f-81a5af4924f6",
                "bd257f3a-eb3b-4b47-a627-3f8f47e82094",
                "ca9091cd-18cc-4837-befa-d962f7995870",
                "879aa714-c57f-40c2-bec2-9606ec5781b2",
                "c05761b1-99d3-406e-9ff2-8a8d5cd3fe78",
                "65df0bd6-82e9-4226-b59c-3555e158a847",
                "482b0cee-0a70-45b8-a8b2-de3e2745074d",
                "6885b48e-1868-4e90-b029-217afd2d94cb",
                "5116668f-0a60-4154-a147-70a3bebdaf61",
                "ed21bd23-e661-4527-bbd5-b87f8a33beb8",
                "04a5274c-abf0-47b3-90b7-4269289402fe",
                "c0b8e883-46a1-4f9e-8e03-cd5302921082",
                "37b3ff41-43ef-4049-a52e-436b211f1edd",
                "f5d71c66-5a7c-4048-b096-a1159f636c66",
                "155396fb-3d8b-4573-b811-3ceeefcbbb2a",
                "f9a76c46-dfb4-44f1-b157-d0e065e48d32",
                "3054e7cf-5dfa-4eda-90a5-8c2a67367b59",
                "afa3963d-f6e8-4017-bf03-869bafa83a40",
                "0ff0a23e-f3f0-4f27-8a0f-4ea55b32acce",
                "7b7eeee4-2573-4688-b56e-53fa809a7e13",
                "de8777c5-5a5c-4702-babe-bf279c7d3bbb",
                "b84c98c5-f7cd-4fac-8111-0e0614da4e95",
                "3acdf0bf-aa07-4e64-bfda-6d67b59586a1",
                "1a249fa0-f59b-4c32-8591-83e4804632b0",
                "368948d7-8f28-4fde-8ab5-f122b978fb74",
                "47e614c5-5f90-4684-8ec9-113ae57a9233",
                "44dc28da-5bd3-491d-a01e-3c66b46fae11",
                "96d5514e-ba23-4838-94e8-45a947184dc2",
                "72d12040-5e0e-4de1-94ba-6badd480cc78",
                "84f71200-c4b8-4715-8570-9e38499e727d",
                "f297c244-d620-4027-9a56-c2a8ce88dc89",
                "038a5849-219c-446c-a267-d47cf758c711",
                "ec4a932e-6def-48ca-8ca5-d75ff30c55f6",
                "16aa9eef-c023-44a9-8811-679f66f770e7",
                "1e996cd2-a90c-4bb6-8c34-2823dc413820",
                "751f2a15-07d3-4253-bb5e-5af599d4e705",
                "4cebda51-5ad2-4f89-9d98-1dc3ac16cf54",
                "372336fc-556c-461e-9ecf-94aa1191b0f5",
                "477878ae-416a-49cb-9132-a4cb2b6808a6",
                "d09731c4-93f2-47cb-a64e-ad7ba4937997",
                "55ac831b-7221-4744-98c9-b862191af704",
                "37aa61e5-9aa2-466c-b706-3917434289c7",
                "cb326ca0-52dc-4829-8328-8cd9307be7c5",
                "d695e49f-1d77-4e20-8b1a-fa090bf42a6d",
                "a3d5846d-6891-4126-b63f-e7ead3c3af36",
                "a9e07ab1-756d-4d4d-91fd-0c51ac9fde33",
                "34319db9-dcb7-4bda-b25e-352fb87a7901",
                "78702f3b-ece8-4089-ba77-212a5fb429dd",
                "4e04278f-f23b-42d3-9af7-1e51f714dd7e",
                "9e1fd91f-dcdc-4d27-91bf-55bb04502cb5",
                "049ecc8f-3406-4023-a8ce-8ce3897e5870",
                "127ad1b9-d2f3-44df-81e5-5bd3c71606f2",
                "a150d973-bee0-483c-bbb8-cb1dc135934a",
                "1657fd2f-73e4-4b17-9ab8-23f2feb757c2",
                "296e5a2b-6485-4f30-a7f2-d0aabead0977",
                "4d3d2368-b03e-49dc-9159-05bb82b64f8d",
                "aea923c0-9f0d-465f-a98f-352b8fec604e",
                "f2f61fcf-d8c6-4f61-b339-c604e436ab79",
                "d53ebd59-5e23-4da1-b514-7751f9ae62e1",
                "997992c5-84ca-499a-ab6a-d1e696e02977",
                "4dc2700b-211b-4fa8-a569-f42782b9ca48",
                "0afb4897-3eb7-479e-90b7-4be75eccdbd9",
                "b98f4499-999d-4cb5-bddd-f3501aca83ce",
                "5a518178-5241-46f4-8c0d-407b6296b1e9",
                "343a3e0a-fcb1-4132-a2a6-df2186a82a45",
                "fdfa1f86-0c55-4b58-95e8-d2829b3cb0f7",
                "9f5f83dc-748c-4063-b76d-6864d8260a32",
                "688ed080-bbab-42e8-9b13-830fe3829bb1",
                "1018ea62-8fe6-4487-a2a3-608341ffc03b",
                "ce3c9f11-1619-4622-a25b-e348ec63c699",
                "73a65490-144b-41a7-a4d6-3d525fd56323",
                "3676af5c-1be0-47e7-94de-76cf332a6397",
                "cbe0f5d2-4328-4273-87e2-e4c56cb409d6",
                "76436e45-4919-4b4d-9d34-6ad382f61760",
                "cdbdbc62-2e47-41b2-8367-64c2a934b548"
            };
        }
    }
}