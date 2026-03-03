using Avalonia.Media;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using cheluan.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace cheluan.Services;

public class TurtleCompletionData : ICompletionData
{
    private readonly DocumentationEntry _entry;

    public TurtleCompletionData(DocumentationEntry entry)
    {
        _entry = entry;
    }

    public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
    {
        textArea.Document.Replace(completionSegment, $"{Text}()");
        textArea.Caret.Offset -= 1; // push cursor inside parentheses ()
    }

    public IImage? Image => null;
    public string Text => _entry.Signature[.._entry.Signature.IndexOf('(')]; // ex. move(distance) -> move (used for automatic writing on Enter)
    public object Content => _entry.Signature; // ex. move(distance) shown in list (visual only)
    public object Description => _entry.Description;
    public double Priority => 0;
}
