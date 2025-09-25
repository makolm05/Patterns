using SingleResponsibility.Correct;

IMobileStore mobileStore = new MobileStore(
    new ConsolePhoneReader(),
    new GeneralPhoneBinder(),
    new GeneralPhoneValidator(),
    new TextPhoneSaver()
);

mobileStore.Process();

// I got the example from metanit.com site for my learning.