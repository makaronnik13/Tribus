public interface ISkillAim
{
    bool IsAwaliable(Card card);
    void Highlight(Card card, bool v);
	void HighlightSimple(bool v);
    void HighlightSelected(Card card, bool v);
}